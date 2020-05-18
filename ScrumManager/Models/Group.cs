using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class Group
    {
        public Group()
        {
            Data = new GroupData();
            Managers = new Dictionary<string, UserData>();
            Writers = new Dictionary<string, UserData>();
            Viewers = new Dictionary<string, UserData>();
        }

        [FirestoreDocumentId]
        public string DocId { get; set; }

        [FirestoreProperty]
        public GroupData Data { get; set; }

        [FirestoreProperty]
        [MinLength(1)]
        public Dictionary<string, UserData> Managers{ get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserData> Writers { get; set; }

        [FirestoreProperty]
        public Dictionary<string, UserData> Viewers { get; set; }
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
