using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Services
{
    public class GroupService
    {
        public async Task Create(Group group)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            WriteBatch batch = db.StartBatch();
            var newGroupDoc = db.Collection("groups").Document();
            batch.Create(newGroupDoc, group);

            foreach (var manager in group.Managers)
            {
                batch.Update(
                    db.Collection("users").Document(manager.Key),
                    new Dictionary<string, object>() { { "Groups." + newGroupDoc.Id, new UserGroupPermissions { Name = group.Data.Name, Roles = new string[] { "Manager" } } } }
                );
            }

            var batchResult = await batch.CommitAsync();
        }
    }
}
