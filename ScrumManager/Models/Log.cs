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
        public string Yesterday { get; set; } = "";

        [FirestoreProperty]
        public string Today { get; set; } = "";

        [FirestoreProperty]
        public string Blockers { get; set; } = "";

        [FirestoreProperty]
        public Timestamp Date { get; set; }

        [FirestoreProperty]
        public string GroupID { get; set; }

        [FirestoreProperty]
        public string UserID { get; set; }

        [FirestoreProperty]
        public string UserName { get; set; }
    }
}
