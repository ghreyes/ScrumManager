using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    public class HomeVM
    {
        public HomeVM() { Groups = new List<HomeGroupDetails>(); }

        public HomeVM(User u)
        {
            Groups = new List<HomeGroupDetails>();

            if (u.Groups != null)
            {
                foreach (var g in u.Groups)
                {
                    Groups.Add(new HomeGroupDetails()
                    {
                        ID = g.Key,
                        Name = g.Value.Name,
                        Roles = g.Value.Roles
                    });
                }
            }
        }

        public List<HomeGroupDetails> Groups { get; set; }
    }

    public class HomeGroupDetails
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
        public bool IsLogComplete { get; set; }
        public int TotalLogsCompelete { get; set; }
        public int TotalWriters { get; set; }
    }
}
