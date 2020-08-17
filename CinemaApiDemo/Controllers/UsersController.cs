using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AuthenticationPlugin;
using CinemaApiDemo.Data;
using CinemaApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDBContext _dbContext;
        private IConfiguration  _configuration;
        private readonly AuthService _auth;

        public UsersController(CinemaDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]   // [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var userWithSameEmail = _dbContext.Users.Where(u=>u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists.");
            }
            var userObj = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                Role = "User"
            };
            _dbContext.Users.Add(userObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null) 
            {
                return NotFound();
            }

            //And to validate the password you need to add the following line of code.
            //first parameter is the password provided by user in plain text
            //second parameter is the hashed password retrieved from the db
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }
            //if login a user with admin role, will return admin token
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, userEmail.Role),//get the role value from db
             };
             //this line will generate access token
             var token = _auth.GenerateAccessToken(claims);
             return new ObjectResult(new
             {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,//you can remove the remaining if you only want to return token type
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id
             });

        }

    }
}
