﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ScrumManager.Models;

namespace ScrumManager.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            const string user_id = "u1";

            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var userSnapshot = await db.Collection("users").Document(user_id).GetSnapshotAsync();
            var homeVM = new HomeVM(userSnapshot.ConvertTo<User>());

            foreach(var group in homeVM.Groups)
            {
                var groupSnapshot = await db.Collection("groups").Document(group.ID).GetSnapshotAsync();
                var groupData = groupSnapshot.ConvertTo<Group>();
                group.TotalWriters = groupData.Writers.Count;

                var logSnapshot = await db.Collection("logs")
                    .WhereEqualTo("GroupID", group.ID)
                    .WhereLessThan("Date", DateTime.Today.AddDays(1).ToUniversalTime())
                    .GetSnapshotAsync();
                //.WhereGreaterThanOrEqualTo("Date", DateTime.Today)
                var logData = logSnapshot.Documents.Select(x => x.ConvertTo<Log>()).ToList();
                group.TotalLogsCompelete = logData.Count;
                group.IsLogComplete = logData.Any(x => x.UserID == user_id);
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}