using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Collections.Generic;
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


            // Update
            var userDoc = db.Collection("users").Document(user.DocId);

            batch.Set(userDoc, user, SetOptions.MergeAll);
            
            var batchResult = await batch.CommitAsync();

            return (await userDoc.GetSnapshotAsync()).ConvertTo<User>();
        }
    }
}
