using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto user)
        {
            if (user.Username != "Xander")
            {
                return BadRequest("User not found");
            }

            if (user.Password != "geheim")
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Trainer"),
                new Claim(ClaimTypes.Role, "Guest")
            };

            // De secret key staat 'normaal gesproken' in de appsettings.json
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Some secret key at least 16 characters"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var handler = new JwtSecurityTokenHandler();

            string jwt = handler.WriteToken(token);

            return jwt;
        }


    }
}
