using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Threading.Tasks;

namespace ScrumManager.Services
{
    public class LogService
    {
        public async Task CreateOrUpdate(Log log)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            WriteBatch batch = db.StartBatch();

            var logDoc = log.DocId == null ? 
                db.Collection("logs").Document() : 
                db.Collection("logs").Document(log.DocId);
            batch.Set(logDoc, log);

            var batchResult = await batch.CommitAsync();
        }
    }
}
