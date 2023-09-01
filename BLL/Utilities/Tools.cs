using DAL.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
//using DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace BLL.Utilities
{
    public static class Tools
    {

        public static List<Claim> GenerateClaims(ApplicationUser res, IList<string> roles, int profileId)
        {
            var claims = new List<Claim>()
                {
                //new Claim(JwtRegisteredClaimNames.Email , res.Email),
                new Claim(ClaimTypes.Name , res.UserName),
                new Claim("PID"  ,profileId.ToString()),
                //new Claim("BuddyID",buddy.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, res.Id),
                };
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        public static string GenerateJWT(List<Claim> claims,string key)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "https://localhost:44310",
               audience: "https://localhost:44310",
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddYears(1),
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );
            string JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return JwtToken;
        }
        public static string GetClaimValue(HttpContext httpContext, string valueType)
        {
            if (string.IsNullOrEmpty(valueType)) return null;

            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                var valueObj = identity == null ? null : identity.Claims.FirstOrDefault(x => x.Type == valueType);
                return valueObj == null ? null : valueObj.Value;
            }
            return null;
        }
        public static int GetIntClaimValue(HttpContext httpContext, string valueType)
        {
            string claimValue = GetClaimValue(httpContext, valueType);
            if (!string.IsNullOrEmpty(claimValue) && int.TryParse(claimValue, out int parsedValue))
            {
                return parsedValue;
            }
            return 0; // Default value if parsing fails or claim is missing
        }

        public static string ImageTypes = ".jpg,.bmp,.PNG,.EPS,.gif,.TIFF,.tif,.jfif";

    }
}
