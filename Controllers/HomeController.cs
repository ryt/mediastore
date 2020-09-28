using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using mediastore.Models;
using mediastore.Models.DataModels;
using mediastore.DataProviders;
using mediastore.Libraries;

namespace mediastore.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private FirestoreProvider firestoreProvider = new FirestoreProvider();

        private StorageProvider storageProvider = new StorageProvider();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var content = "";
            var currentTeacherId = Request.Cookies["teacher"]; // also in BaseController


            // upload media file with associated teacher
            
            if ( Request.Method == "POST" && Request.Form.Files.Count > 0 ) 
            {
                var uploadedFile = Request.Form.Files[0];
                var mediafile = new Mediafile
                {
                    OriginalFileName = uploadedFile.FileName,
                    Title = uploadedFile.FileName,
                    ContentType = uploadedFile.ContentType,
                    TeacherId = currentTeacherId
                };
                var mediafileId = await firestoreProvider.AddMediafileAsync(mediafile);
                using ( var memoryStream = new MemoryStream() )
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    var obj = new Storagefile
                    {
                        Name = mediafileId, // Path.GetExtension if we want the .mp4
                        ContentType = uploadedFile.ContentType, // content type is set so we don't really need the extension
                        Metadata = new Dictionary<string, string>
                        {
                            { "OriginalFileName", uploadedFile.FileName }
                        }
                    };
                    var upload = await storageProvider.UploadObjectAsync(obj, memoryStream);
                    ViewBag.uploadedData = upload;
                }
            }
            

            // display list of media files for current teacher

            if ( !String.IsNullOrEmpty(currentTeacherId) ) {
                var listFiles = await firestoreProvider.GetMediafilesByTeacherIdAsync(currentTeacherId);
                listFiles.Select(c => { c.HumanTime = Utility.RelativeDate(c.CreatedDateTime); return c; }).ToList();
                listFiles.Sort((y, x) => DateTime.Compare(x.CreatedDateTime, y.CreatedDateTime)); // newest first
                ViewBag.listFiles = listFiles;
                ViewBag.bucketName = storageProvider.bucketName;
            }


            ViewBag.content = content;
            return View();
        }

        [Route("media")]
        public IActionResult Media()
        {
            return View();
        }

        [Route("media/{mediaId}")]
        public async Task<IActionResult> Media(string mediaId)
        {
            Mediafile mediafile = await firestoreProvider.GetMediafileByIdAsync(mediaId);
            ViewBag.mediafile = mediafile;
            ViewBag.bucketName = storageProvider.bucketName;
            return View();
        }

        [Route("media/{mediaId}/edit")]
        public async Task<IActionResult> Media(string mediaId, string edit)
        {
            var currentTeacherId = Request.Cookies["teacher"];
            Mediafile mediafile = await firestoreProvider.GetMediafileByIdAsync(mediaId);

            if ( mediafile.TeacherId == currentTeacherId ) {

                // edit media file

                if ( Request.Method == "POST" &&  Request.Form["mediaEditSubmit"] == "true" ) {
                    ViewBag.submitted = true;
                    string title = Request.Form["mediaTitle"];
                    mediafile.Title = title;
                    var update = await firestoreProvider.UpdateMediafileByIdAsync(mediaId, title);
                    if ( update != null ) {
                        ViewBag.editSuccess = true;
                    }
                }

                
                ViewBag.edit = true;
                ViewBag.mediafile = mediafile;
                ViewBag.bucketName = storageProvider.bucketName;

            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
