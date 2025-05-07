using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace REST_API.Controllers.Utils
{
    public class AuthOptions
    {
        private const string ISSUER = "MyAuthServer"; // издатель токена
        private const string AUDIENCE = "MyAuthClient"; // потребитель токена
        private const string KEY = "mysupersecret_secretkey_super_super_long_key";   // ключ для шифрации
        private static TimeSpan LIFETIME = TimeSpan.FromMinutes(5); // время жизни токена - 1 минута
        private static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(KEY));

        public static TokenValidationParameters TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = ISSUER,
            ValidateAudience = true,
            ValidAudience = AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };

        public static JwtSecurityToken GenerateJwt(List<Claim> claims, TimeSpan? lifetime = null)
        {
            var now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(lifetime ?? LIFETIME),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
        }
        public static string SerializeJwtToken(JwtSecurityToken jwt) => new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
