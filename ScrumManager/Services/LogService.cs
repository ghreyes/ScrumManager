using Google.Cloud.Firestore;
using ScrumManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumManager.Services
{
    public class LogService
    {
        public async Task<Log> CreateOrUpdate(Log log)
        {
            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            WriteBatch batch = db.StartBatch();

            DocumentReference logDoc;
            if(log.DocId == null)
            {
                // Create
                logDoc = db.Collection("logs").Document();
                batch.Set(logDoc, log);
            }
            else
            {
                // Update
                logDoc = db.Collection("logs").Document(log.DocId);

                batch.Update(logDoc, new Dictionary<string, object>
                {
                    { "Blockers", log.Blockers },
                    { "Today", log.Today },
                    { "Yesterday", log.Yesterday }
                });
            }

            var batchResult = await batch.CommitAsync();

            return (await logDoc.GetSnapshotAsync()).ConvertTo<Log>();
        }
    }
}
