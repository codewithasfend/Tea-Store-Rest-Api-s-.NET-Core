using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository userRepository;
        private IConfiguration config;
        public UsersController(IUserRepository userRepository, IConfiguration config)
        {
            this.userRepository = userRepository;
            this.config = config;

        }

        // api/Users/Register

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var registrationResult = await userRepository.RegisterUser(user);
            if (registrationResult)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest();
        }

        // api/Users/Login

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var loggedInUser = await userRepository.LoginUser(user.Email, user.Password);
            if (loggedInUser == null)
            {
                return NotFound("User not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.Email,user.Email),
                 new Claim(ClaimTypes.Role, loggedInUser.Role)
            };

            var token = new JwtSecurityToken(
                    issuer: config["JWT:Issuer"],
                    audience: config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(60),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                access_token = jwt,
                token_type ="bearer",
                user_id = loggedInUser.Id,
                user_name = loggedInUser.Name,
            });

        }

    }
}
