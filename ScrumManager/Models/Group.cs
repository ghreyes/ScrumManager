using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class Group
    {
        public Group()
        {
            Data = new GroupData();
            Users = new Dictionary<string, Group_UserData>();
        }

        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public GroupData Data { get; set; }

        [FirestoreProperty]
        [MinLength(1)]
        public Dictionary<string, Group_UserData> Users { get; set; }

        [FirestoreProperty]
        public Dictionary<string, Group_InviteData> Invites { get; set; }

        public Dictionary<string, Group_UserData> Managers => Users.Where(x => x.Value.Roles.Contains("Manager")).ToDictionary(x => x.Key, x => x.Value);
        public Dictionary<string, Group_UserData> Writers => Users.Where(x => x.Value.Roles.Contains("Writer")).ToDictionary(x => x.Key, x => x.Value);
        public Dictionary<string, Group_UserData> Viewers => Users.Where(x => x.Value.Roles.Contains("Viewer")).ToDictionary(x => x.Key, x => x.Value);
    }

    [FirestoreData]
    public class GroupData
    {
        [FirestoreProperty]
        [Required]
        [StringLength(50, ErrorMessage = "Group name must be between {2} and {1} characters.", MinimumLength = 5)]
        public string Name { get; set; }
    }
}
