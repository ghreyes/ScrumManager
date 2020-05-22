using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    public class GroupVM
    {
        public GroupVM(Group g)
        {
            DocId = g.DocId;
            Data = g.Data;
            Users = g.Users;
            Logs = new Dictionary<string, Log>();
        }

        public string DocId { get; set; }

        public GroupData Data { get; set; }

        public Dictionary<string, Group_UserData> Users { get; set; }

        public Dictionary<string, Log> Logs { get; set; }
    }
}
