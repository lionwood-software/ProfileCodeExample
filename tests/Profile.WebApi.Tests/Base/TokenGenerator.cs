using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Profile.WebApi.Tests.Base
{
    public static class TokenGeneration
    {
        public const string Key = "Profile.WebApi.Tests";

        public static string BuildIndividualToken(AuthOptions options, string email, Guid userId)
        {
            var claims = new[]
            {
                new Claim("scope", "individual"),
                new Claim("email", email),
                new Claim("Sub", userId.ToString()),
            };

            return CreateToken(options, claims);
        }

        public static string BuildServiceToServiceToken(AuthOptions options)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim("scope", "maintenance")
            };

            return CreateToken(options, claims);
        }

        private static string CreateToken(AuthOptions options, Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                options.Authority,
                options.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}