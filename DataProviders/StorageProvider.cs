using System;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using mediastore.Models.DataModels;

namespace mediastore.DataProviders
{
    public class StorageProvider
    {
        public string bucketName = "mediastore-289101.appspot.com";
        private static StorageClient storage = StorageClient.Create();


        public async Task<string> UploadObjectAsync(Storagefile file, MemoryStream memoryStream)
        {
            var obj = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = bucketName,
                Name = file.Name,
                ContentType = file.ContentType,
                Metadata = file.Metadata
            };
            var upload = await storage.UploadObjectAsync(obj, memoryStream);
            return upload.MediaLink;
        }

        // random test methods

        public string ListFilesAsHTML()
        {
            var content = "";
            foreach ( var storageObject in storage.ListObjects(bucketName, "") )
            {
                content += String.Format("<a href=\"https://storage.googleapis.com/{0}/{1}\">{1}</a><br>", bucketName, storageObject.Name);
            }
            return content;
        }


    }
}