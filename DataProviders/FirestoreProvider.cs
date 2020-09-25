using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Google.Cloud.Firestore;
using mediastore.Models.DataModels;

namespace mediastore.DataProviders
{
    public class FirestoreProvider
    {
        private static FirestoreDb db = FirestoreDb.Create("mediastore-289101");

        public async Task<string> AddMediafileAsync(Mediafile mediafile)
        {
            var docRef = db.Collection("mediafiles");
            var document = await docRef.AddAsync(mediafile);

            return document.Id;
        }

        public async Task<List<Mediafile>> GetMediafilesByTeacherIdAsync(string teacherId)
        {
            var mediafiles = new List<Mediafile>();

            var docRef = db.Collection("mediafiles").WhereEqualTo("TeacherId", teacherId);
            var mediafilesSnap = await docRef.GetSnapshotAsync();

            if ( mediafilesSnap.Any() ) 
            {
                mediafiles = mediafilesSnap.Select(x => x.ConvertTo<Mediafile>()).ToList();
            }
            
            return mediafiles; 
        }


        public async Task<Mediafile> GetMediafileByIdAsync(string mediafileId)
        {
            Mediafile mdf = null;

            var docRef = db.Collection("mediafiles").Document(mediafileId);
            var mdfSnap = await docRef.GetSnapshotAsync();

            if ( mdfSnap.Exists ) 
            {
                mdf = mdfSnap.ConvertTo<Mediafile>();
            }
            
            return mdf;
        }

        // random test methods

        public async Task<string> ListUsersAsHTML()
        {
            var content = "";
            QuerySnapshot allUsersQuerySnapshot = await db.Collection("users").GetSnapshotAsync();
            foreach ( DocumentSnapshot documentSnapshot in allUsersQuerySnapshot.Documents )
            {
                content += String.Format("<b>{0}:</b><br>", documentSnapshot.Id);
                Dictionary<string, object> user = documentSnapshot.ToDictionary();
                foreach ( KeyValuePair<string, object> pair in user )
                {
                    content += String.Format(" - {0} : {1}<br>", pair.Key, pair.Value);
                }
            }
            return content;
        }

    }
}