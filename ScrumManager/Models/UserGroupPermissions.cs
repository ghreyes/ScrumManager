using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ScrumManager.Models
{
    [FirestoreData]
    public class UserGroupPermissions
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string[] Roles { get; set; }
    }
}
