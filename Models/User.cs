using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace REST_API.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Admin = 0,User = 1, Viewer =2
        /// </summary>
        public string Role { get; set; }
    }

   /* public enum Role
    {
        Admin,
        User,
        Viewer
    }*/
}
