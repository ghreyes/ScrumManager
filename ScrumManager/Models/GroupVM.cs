using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    public class GroupVM : Group
    {
        public GroupVM(Group g)
        {
            DocId = g.DocId;
            Data = g.Data;
            Users = g.Users;
            Invites = g.Invites;
            Logs = new Dictionary<string, Log>();
        }

        public Dictionary<string, Log> Logs { get; set; }

        public string UserID { get; set; }
    }
}
