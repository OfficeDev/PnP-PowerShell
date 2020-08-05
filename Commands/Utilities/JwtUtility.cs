using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities
{
    internal static class JwtUtility
    {
        public static bool HasScope(string accessToken, string scope)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var scpClaims = token.Claims.Where(c => c.Type == "scp");
            var roleClaims = token.Claims.Where(c => c.Type == "roles");
            if (scpClaims != null)
            {
                foreach (var scpClaim in scpClaims)
                {
                    if (scpClaim.Value.Contains(scope))
                    {
                        return true;
                    }
                }
            }
            if (roleClaims != null)
            {
                foreach (var roleClaim in roleClaims)
                {
                    if (roleClaim.Value.Contains(scope))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
