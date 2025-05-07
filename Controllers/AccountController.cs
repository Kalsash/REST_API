using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REST_API.Controllers.Utils;
using REST_API.Models;
using REST_API.Models.Contexts;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace REST_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
 

        AuthContext _context;

        public AccountController(AuthContext _db)
        {
            _context = _db;
        }
        // GET: api/<AccountController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.Users.AsNoTracking().ToListAsync());
        }
        [HttpPost("/token")]
        public async Task<IActionResult> Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            // создаем JWT-токен
            var jwt = AuthOptions.GenerateJwt(identity.Claims.ToList());
            var encodedJwt = AuthOptions.SerializeJwtToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = _context.Users.First(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            // если пользователя не найдено
            return null;
        }
       
        /// <summary>
        /// 0 - admin
        /// 1 - user
        /// 3 - viewer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> SignUp(string username, string password, string role)
        {
            try
            {
                _context.Users.Add(new User()
                {
                    Login = username,
                    Password = password,
                    Role = role

                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(username))
                    return Conflict();
                else
                    throw;
            }
            return Ok();
        }
        private bool UserExists(string login) =>
            _context.Users.Any(e => e.Login == login);
    }
}
