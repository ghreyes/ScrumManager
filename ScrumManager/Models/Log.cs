using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class Log
    {
        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public string Content { get; set; }

        [FirestoreProperty]
        public Timestamp Date { get; set; }

        [FirestoreProperty]
        public string GroupID { get; set; }

        [FirestoreProperty]
        public string UserID { get; set; }
    }
}
