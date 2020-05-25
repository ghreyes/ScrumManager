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

            try
            {
                await _logService.CreateOrUpdate(log);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
