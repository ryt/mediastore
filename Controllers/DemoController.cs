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
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;

namespace mediastore.Controllers
{
    public class DemoController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public DemoController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var content = "";
            var gcloudId = "mediastore-289101";
            var bucketName = gcloudId + ".appspot.com";

            // Firestore Connection
            FirestoreDb db = FirestoreDb.Create(gcloudId);

            Query allUsersQuery = db.Collection("users");
            QuerySnapshot allUsersQuerySnapshot = await allUsersQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allUsersQuerySnapshot.Documents)
            {
                content += "<b>" + documentSnapshot.Id + ":</b>" + "<br>";
                Dictionary<string, object> user = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in user)
                {
                    content += " - " + pair.Key + " : " + pair.Value + "<br>";
                }
            }
            content += "<hr>";

            // Storage Connection
            StorageClient storage = StorageClient.Create();
            foreach (var storageObject in storage.ListObjects(bucketName, ""))
            {
                content += "<a href=\"https://storage.googleapis.com/" + bucketName + "/" + storageObject.Name + "\">" + storageObject.Name + "</a><br>";
            }


            // Simple File Upload with POST
            if ( Request.Method == "POST" && Request.Form.Files.Count > 0 ) 
            {
                var uploadedFile = Request.Form.Files[0];
                using (var memoryStream = new MemoryStream())
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    var obj = new Google.Apis.Storage.v1.Data.Object
                    {
                        Bucket = bucketName,
                        Name = uploadedFile.FileName,
                        ContentType = uploadedFile.ContentType,
                        Metadata = new Dictionary<string, string>
                        {
                            { "originalFileName", uploadedFile.FileName }
                        }
                    };
                    var dataObject = await storage.UploadObjectAsync(obj, memoryStream);
                    ViewBag.uploadedData = dataObject.MediaLink;
                }
            }

            ViewBag.content = content;
            return View();
        }

    }
}
