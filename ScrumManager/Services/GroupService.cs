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

        public async Task EditUsers(Group group)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            WriteBatch batch = db.StartBatch();
            var groupDoc = db.Collection("groups").Document(group.DocId);

            var currentGroup = (await groupDoc.GetSnapshotAsync()).ConvertTo<Group>();

            // Remove group from removed users
            foreach(var user in currentGroup.Users)
            {
                if (!group.Users.ContainsKey(user.Key))
                {
                    var userDoc = db.Collection("users").Document(user.Key);
                    batch.Update(userDoc, new Dictionary<FieldPath, object>
                    {
                        { new FieldPath("Groups", currentGroup.DocId), FieldValue.Delete }
                    });
                }
            }

            // Add group to new users 
            // AND edit permissions for current users
            foreach (var user in group.Users)
            {
                var userDoc = db.Collection("users").Document(user.Key);
                batch.Update(userDoc, new Dictionary<FieldPath, object>
                {
                    { new FieldPath("Groups", group.DocId), new { currentGroup.Data.Name, user.Value.Roles } }
                });
            }

            // Send email to new invites
            if (group.Invites != null)
            {
                foreach (var inv in group.Invites)
                {
                    if (!currentGroup.Invites.ContainsKey(inv.Key))
                    {
                        var body = "You have been added to " + currentGroup.Data.Name + ". Register at https://localhost:44347/Register to accept any pending invitations.";
                        var result = EmailService.SendEmail(inv.Value.Email, "Group Invitation for " + currentGroup.Data.Name, body);
                        if (!result.IsSuccessful)
                            System.Diagnostics.Debug.WriteLine(result.ErrorMessage);
                    }
                }
            }

            // Update group's Users/Invites
            batch.Update(groupDoc, new Dictionary<FieldPath, object>
            {
                { new FieldPath("Users"), group.Users },
                { new FieldPath("Invites"), group.Invites }
            });

            var batchResult = await batch.CommitAsync();
        }
    }
}
