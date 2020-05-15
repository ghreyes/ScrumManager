using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class Group
    {
        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public GroupData Data { get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserData> Managers{ get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserData> Writers { get; set; }
    }

    [FirestoreData]
    public class GroupData
    {
        [FirestoreProperty]
        public string Name { get; set; }
    }
}
