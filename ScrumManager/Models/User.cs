using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public string FirstName { get; set; }

        [FirestoreProperty]
        public string LastName { get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserGroupPermissions> Groups { get; set; }
    }

    /// <summary>
    /// For keeping track of user roles in a group
    /// </summary>
    [FirestoreData]
    public class Group_UserData
    {
        [FirestoreProperty]
        public string FirstName { get; set; }

        [FirestoreProperty]
        public string LastName { get; set; }

        [FirestoreProperty]
        public string[] Roles { get; set; }

    }
}
