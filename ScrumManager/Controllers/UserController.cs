using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ScrumManager.Hubs;
using ScrumManager.Models;
using ScrumManager.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumManager.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private UserService _userService;


        public UserController(LogHub logHub)
        {
            _userService = new UserService();
        }

        [HttpPost("GetUserByEmail")]
        public async Task<IActionResult> GetUserIdByEmail()
        {
            try
            {
                var email = Request.Form["email"];
                if (email.Count == 0)
                {
                    return BadRequest();
                }

                return Json(await _userService.GetUserIdByEmail(email));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

    }
}
