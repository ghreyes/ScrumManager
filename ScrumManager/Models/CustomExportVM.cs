using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrumManager.Models
{
    public class CustomExportVM 
    {
        public CustomExportVM(Group g, CustomExportForm f)
        {
            GroupID = g.DocId;
            GroupData = g.Data;
            Users = g.Users;
            //Invites = g.Invites.Select(x => x.Value).ToList();
            Logs = new List<Log>();
            FormData = f;
        }

        public string GroupID { get; set; }
        public GroupData GroupData { get; set; }
        public Dictionary<string, Group_UserData> Users { get; set; }
        //public List<Group_InviteData> Invites { get; set; }
        public List<Log> Logs { get; set; }

        public string UserID { get; set; }
       
        public CustomExportForm FormData { get; set; }
    }
}
