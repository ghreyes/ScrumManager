using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ScrumManager.Hubs;
using ScrumManager.Models;
using ScrumManager.Services;

namespace ScrumManager.Controllers
{
    [Route("[controller]")]
    public class GroupController : Controller
    {
        private readonly LogHub _logHub;
        private FirestoreChangeListener _listener;
        private UserService _userService;
        private GroupService _groupService;


        public GroupController(LogHub logHub)
        {
            _logHub = logHub;
            _userService = new UserService();
            _groupService = new GroupService();
        }

        [Route("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return View();

            if (!await _userService.IsUserInGroup(User.GetUserID(), id))
            {
                ViewData["URL"] = Request.Host.ToString() + Request.Path;
                return View("Unauthorized");
            }

            string credential_path = @"C:\Users\ghrey\Downloads\ScrumManager-c7ce2bf2810c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            FirestoreDb db = FirestoreDb.Create("scrummanager");
            var data = await db.Collection("groups").Document(id).GetSnapshotAsync();
            var group = data.ConvertTo<Group>();

            var groupVM = new GroupVM(group);

            foreach(var u in groupVM.Users)
            {
                if (!u.Value.Roles.Contains("Writer")) continue;
                var logData = await db.Collection("logs")
                    .WhereEqualTo("UserID", u.Key)
                    .WhereGreaterThanOrEqualTo("Date", DateTime.Now.ToUniversalTime().Date)
                    .WhereLessThan("Date", DateTime.Now.AddDays(1).ToUniversalTime().Date)
                    .GetSnapshotAsync(); 
                var logDoc = logData.FirstOrDefault();

                if (logDoc != null)
                {
                    var log = logDoc.ConvertTo<Log>();
                    groupVM.Logs.Add(log.DocId, log);
                }
            }

            var query = db.Collection("logs")
                    .WhereEqualTo("GroupID", id)
                    .WhereGreaterThanOrEqualTo("Date", DateTime.Now.ToUniversalTime().Date)
                    .WhereLessThan("Date", DateTime.Now.AddDays(1).ToUniversalTime().Date);

            var hubChannel = id + "_" + DateTime.Now.ToString("yyyyMMdd");

            _listener = query.Listen(async snapshot =>
            {
                foreach (DocumentChange change in snapshot.Changes)
                {
                    if (change.ChangeType.ToString() == "Added")
                    {
                        await _logHub.LogAdded(hubChannel, change.Document.ConvertTo<Log>());
                    }
                    else if (change.ChangeType.ToString() == "Modified")
                    {
                        await _logHub.LogModified(hubChannel, change.Document.ConvertTo<Log>());
                    }
                    else if (change.ChangeType.ToString() == "Removed")
                    {
                        await _logHub.LogRemoved(hubChannel, change.Document.ConvertTo<Log>());
                    }
                }
            });

            groupVM.UserID = User.GetUserID();

            return View(groupVM);
        }

        [HttpGet("ChangeListener/{groupToJoin}")]
        public async Task<IActionResult> ChangeListener(string groupToJoin)
        {
            try
            {
                if(_listener != null) await _listener.StopAsync();
                var provider = new DateTimeFormatInfo();
                provider.LongDatePattern = "yyyyMMdd";
                var date = DateTime.ParseExact(groupToJoin.Split('_')[1], "yyyyMMdd", null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                var groupID = groupToJoin.Split('_')[0];
                FirestoreDb db = FirestoreDb.Create("scrummanager");

                var query = db.Collection("logs")
                   .WhereEqualTo("GroupID", groupID)
                   .WhereGreaterThanOrEqualTo("Date", date.Date)
                   .WhereLessThan("Date", date.AddDays(1).Date);

                _listener = query.Listen(async snapshot =>
                {
                    foreach (DocumentChange change in snapshot.Changes)
                    {
                        if (change.ChangeType == DocumentChange.Type.Added)
                        {
                            await _logHub.LogAdded(groupToJoin, change.Document.ConvertTo<Log>());
                        }
                        else if (change.ChangeType == DocumentChange.Type.Modified)
                        {
                            await _logHub.LogModified(groupToJoin, change.Document.ConvertTo<Log>());
                        }
                        else if (change.ChangeType == DocumentChange.Type.Removed)
                        {
                            await _logHub.LogRemoved(groupToJoin, change.Document.ConvertTo<Log>());
                        }
                    }
                });

                var qs = await query.GetSnapshotAsync();
                return Json(qs.Documents.Select(x => x.ConvertTo<Log>()));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Group group)
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
