﻿using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ScrumManager.Models;
using ScrumManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Controllers
{

    public class HomeController : Controller
    {
        GroupService _groupService;
        UserService _userService;

        public HomeController()
        {
            _groupService = new GroupService();
            _userService = new UserService();
        }

        [HttpGet("Home")]
        public async Task<IActionResult> Index()
        {
            const string user_id = "u1";

            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var userSnapshot = await db.Collection("users").Document(user_id).GetSnapshotAsync();
            var user = userSnapshot.ConvertTo<User>();

            var homeVM = new HomeVM(user);
            foreach (var group in homeVM.Groups)
            {
                var groupSnapshot = await db.Collection("groups").Document(group.ID).GetSnapshotAsync();
                var groupData = groupSnapshot.ConvertTo<Group>();
                group.TotalWriters = groupData.Writers.Count;

                var logSnapshot = await db.Collection("logs")
                    .WhereEqualTo("GroupID", group.ID)
                    .WhereGreaterThanOrEqualTo("Date", DateTime.Now.ToUniversalTime().Date)
                    .WhereLessThan("Date", DateTime.Now.AddDays(1).ToUniversalTime().Date)
                    .GetSnapshotAsync();
                var logData = logSnapshot.Documents.Select(x => x.ConvertTo<Log>()).ToList();
                group.TotalLogsCompelete = logData.Count;
                group.IsLogComplete = logData.Any(x => x.UserID == user_id);
            }

            ViewBag.AddGroupModel = new Group
            {
                Users = new Dictionary<string, Group_UserData> {
                    {
                        user.DocId, new Group_UserData {
                            DisplayName = user.DisplayName,
                            Roles = new string[]{"Manager"}
                        }
                    }
                }
            };

            return View(homeVM);
        }

        [HttpPost("Home/CreateGroup")]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            var f = Request.Form;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _groupService.Create(group);
                return Ok();
            }
            catch(Exception ex)
            {
                return Error();
            }
        }

        [HttpGet("[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("[action]")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var form = Request.Form;

            try
            {
                await _userService.Update(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return Error();
            }
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
