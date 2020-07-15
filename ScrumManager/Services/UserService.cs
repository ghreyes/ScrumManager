using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Services
{
    public class UserService
    {
        public async Task<User> Update(User user)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            WriteBatch batch = db.StartBatch();

            var userDoc = db.Collection("users").Document(user.DocId);
            if (user.Groups == null) 
                user.Groups = new Dictionary<string, UserGroupPermissions>();
            batch.Set(userDoc, user, SetOptions.MergeAll);

            // Find docs where user is in Invites
            var allGroups = await db.Collection("groups").GetSnapshotAsync();
            var inviteGroups = allGroups.Documents
                .Select(x => x.ConvertTo<Group>())
                .Where(x => x.Invites != null && x.Invites.ContainsKey(user.Email))
                .ToList();

            foreach (var g in inviteGroups)
            {
                // Convert Invites to Users
                var groupRef = db.Collection("groups").Document(g.DocId);
                batch.Update(groupRef, new Dictionary<FieldPath, object>
                {
                    { new FieldPath("Users", user.DocId), new { user.DisplayName, g.Invites[user.Email].Roles } },
                    { new FieldPath("Invites", user.Email), FieldValue.Delete }
                });

                //Add Groups to User
                batch.Update(userDoc, new Dictionary<string, object>
                {
                    { "Groups." + g.DocId, new { g.Data.Name, g.Invites[user.Email].Roles } }
                });
            }
                        
            var batchResult = await batch.CommitAsync();

            return (await userDoc.GetSnapshotAsync()).ConvertTo<User>();
        }

        public async Task<bool> IsUserInGroup(string UserID, string GroupID)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var data = await db.Collection("groups").Document(GroupID).GetSnapshotAsync();
            var group = data.ConvertTo<Group>();

            return IsUserInGroup(UserID, group);
        }

        public bool IsUserInGroup(string UserID, Group group)
        {
            return group.Users.ContainsKey(UserID);
        }

        public async Task<User> GetUserIdByEmail(string email)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var data = await db.Collection("users").WhereEqualTo("Email", email).GetSnapshotAsync();

            if (data.Documents.Count == 0) return new User();
            else return data.Documents[0].ConvertTo<User>();;
        }

        //public async Task Test(User user)
        //{
        //    string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

        //    FirestoreDb db = FirestoreDb.Create("scrummanager");
        //    WriteBatch batch = db.StartBatch();

        //    // Find docs where user is in Invites
        //    var allGroups = await db.Collection("groups").GetSnapshotAsync();
        //    var inviteGroups = allGroups.Documents
        //        .Select(x => x.ConvertTo<Group>())
        //        .Where(x => x.Invites != null && x.Invites.ContainsKey(user.Email))
        //        .ToList();

        //    foreach (var g in inviteGroups)
        //    {
        //        // Convert Invites to Users
        //        var groupRef = db.Collection("groups").Document(g.DocId);
        //        batch.Update(groupRef, new Dictionary<FieldPath, object>
        //        { 
        //            { new FieldPath("Users", user.DocId), new { user.DisplayName, g.Invites[user.Email].Roles } },
        //            { new FieldPath("Invites", user.Email), FieldValue.Delete }
        //        });

        //        //Add Groups to User
        //        var userRef = db.Collection("users").Document(user.DocId);
        //        batch.Update(userRef, new Dictionary<string, object>
        //        {
        //            { "Groups." + g.DocId, new { g.Data.Name, g.Invites[user.Email].Roles } }
        //        });
        //        break;
        //    }

        //    await batch.CommitAsync();
        //    return;
        //}
    }
}
