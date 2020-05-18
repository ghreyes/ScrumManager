using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Threading.Tasks;

namespace ScrumManager.Services
{
    public class GroupService
    {
        public async Task<bool> Create(Group group)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var newDoc = await db.Collection("groups").AddAsync(group);
            return newDoc.Id != null;
        }
    }
}
