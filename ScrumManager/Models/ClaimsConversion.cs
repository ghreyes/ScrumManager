using FirebaseAdmin.Auth;
using System.Collections.Generic;
using System.Security.Claims;

namespace ScrumManager.Models
{
    public static class ClaimsConversion
    {
        public static List<Claim> ToClaims(this FirebaseToken token)
        {
            var claims = new List<Claim>();
            foreach(var claim in token.Claims)
            {
                claims.Add(new Claim(type: claim.Key, value: claim.Value.ToString()));
            }
            return claims;
        }
    }
}
