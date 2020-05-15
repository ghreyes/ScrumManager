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
            Managers = g.Managers;
            Writers = g.Writers;
            Logs = new Dictionary<string, string>();
        }

        public string DocId { get; set; }

        public GroupData Data { get; set; }

        public Dictionary<string, UserData> Managers { get; set; }

        public Dictionary<string, UserData> Writers { get; set; }

        public Dictionary<string,string> Logs { get; set; }
    }
}
