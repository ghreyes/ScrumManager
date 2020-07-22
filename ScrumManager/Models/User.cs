using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public string DisplayName { get; set; }

        [FirestoreProperty]
        [EmailAddress]
        public string Email { get; set; }

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
        public string DisplayName { get; set; }

        [FirestoreProperty]
        public string[] Roles { get; set; }
    }

    [FirestoreData]
    public class Group_InviteData
    {
        [FirestoreProperty]
        [EmailAddress]
        public string Email { get; set; }

        [FirestoreProperty]
        public string[] Roles { get; set; }
    }
}
