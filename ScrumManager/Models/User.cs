using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public UserData Data { get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserGroupPermissions> Groups { get; set; }
    }

    [FirestoreData]
    public class UserData
    {
        [FirestoreProperty]
        public string FirstName { get; set; }

        [FirestoreProperty]
        public string LastName { get; set; }
    }
}
