﻿using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ScrumManager.Models;
using ScrumManager.Services;
using System;
using System.Threading.Tasks;

namespace ScrumManager.Controllers
{
    [Route("[controller]")]
    public class LogController : Controller
    {
        LogService _logService;

        public LogController()
        {
            _logService = new LogService();
        }

        [HttpPost("UpdateLog")]
        public async Task<IActionResult> UpdateLog(Log log)
        {
            var f = Request.Form;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // New Log
            if(log.DocId == null)
            {
                log.Date = Timestamp.FromDateTime(DateTime.Parse(Request.Form["FormDate"].ToString()).ToUniversalTime().Date);
                log.UserID = UserManager.GetUserID();
                log.UserName = UserManager.GetUserName();
            }

            try
            {
                return Json(await _logService.CreateOrUpdate(log));
                //return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
