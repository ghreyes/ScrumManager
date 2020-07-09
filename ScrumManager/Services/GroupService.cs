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

            foreach (var u in group.Users)
            {
                batch.Update(
                    db.Collection("users").Document(u.Key),
                    new Dictionary<string, object>() { { "Groups." + newGroupDoc.Id, new UserGroupPermissions { Name = group.Data.Name, Roles = u.Value.Roles } } }
                );
            }

            if (group.Invites != null)
            {
                foreach (var i in group.Invites)
                {
                    var body = "You have been added to " + group.Data.Name + ". Register at https://localhost:44347/Register to accept any pending invitations.";
                    var result = EmailService.SendEmail(i.Value.Email, "Group Invitation for " + group.Data.Name, body);
                    if (!result.IsSuccessful)
                        System.Diagnostics.Debug.WriteLine(result.ErrorMessage);
                }
            }

            var batchResult = await batch.CommitAsync();
        }
    }
}
