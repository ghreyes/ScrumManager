﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ScrumManager.Models;

namespace ScrumManager.Controllers
{
    public class GroupController : Controller
    {
        [Route("/group/index/{id}")] //https://localhost:44347/group/index/testGroup
        public async Task<IActionResult> Index(string id)
        {
            if (id == null) return View();

            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var data = await db.Collection("groups").Document(id).GetSnapshotAsync();
            var group = data.ConvertTo<Group>();

            var groupVM = new GroupVM(group);

            foreach(var u in groupVM.Writers)
            {
                var logData = await db.Collection("logs").WhereEqualTo("UserID", u.Key).GetSnapshotAsync(); //NEEDS DATE FILTER
                var log = logData.First().ConvertTo<Log>();
                groupVM.Logs.Add(log.DocId, log.Content);
            }

            return View(groupVM);
        }
    }
}