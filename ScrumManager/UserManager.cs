using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScrumManager
{
    public static class UserManager
    {
        public static string GetUserID(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("user_id");
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("name");

        }
    }
}
