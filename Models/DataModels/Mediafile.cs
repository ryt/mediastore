using System;
using Google.Cloud.Firestore;

namespace mediastore.Models.DataModels
{
    [FirestoreData]
    public class Mediafile
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string OriginalFileName { get; set; }

        [FirestoreProperty]
        public string ContentType { get; set; }

        [FirestoreProperty]
        public string TeacherId { get; set; }

        [FirestoreProperty]
        public string RubricId { get; set; }

        [FirestoreDocumentCreateTimestamp]
        public DateTime CreatedDateTime { get; set; }

        public string HumanTime { get; set; }

    }
}