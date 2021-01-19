using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using chat_server.Entity.interfaces;
using chat_server.Entity;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
        private IAuth<UserVM> _auth;
        private readonly IConfiguration _config;
		public AuthController(IConfiguration config, IAuth<UserVM> auth)
		{
            _config = config;
            _auth = auth;
        }
        [HttpPost("register")]
        public async Task<Result> Register([FromBody] UserVM value)
        {
            return await _auth.Register(value);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserVM model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                model.ForLogin = true;
                return await loginDetails(model);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(error: ex.Message.ToString());
            }

        }
        [HttpPost("logout")]
        public async Task<bool> Logoff()
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpContext.Session.Clear();
                });
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        async Task<IActionResult> loginDetails(UserVM model)
        {
            try
            {
                string email = "", userId = "", firstName = "", lastName = "";
                Result result = await _auth.Login(model);
                if (result != null)
                {
                    if (result.IsSuccess && result.Data != null)
                    {
                        UserVM userData = (UserVM)result.Data;
                        email = ((UserVM)result.Data).Email;
                        userId = ((UserVM)result.Data).UserId.ToString();
                        firstName = ((UserVM)result.Data).FirstName;
                        lastName = ((UserVM)result.Data).LastName;
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim("USERID", !string.IsNullOrEmpty(userId) ? userId : ""),
                            new Claim("EMAIL", !string.IsNullOrEmpty(email) ? email : "")
                    };
                        string tokenValue = _config.GetSection("AppSettings:Token").Value;
                        byte[] tokenByte = Encoding.UTF8.GetBytes(tokenValue);
                        var key = new SymmetricSecurityKey(tokenByte);

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                        var JWToken = new JwtSecurityToken(
                             issuer: "https://localhost:44319/",
                             audience: "https://localhost:44319/",
                             claims: GetUserClaims(userData),
                             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                             expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                             signingCredentials: creds
                         );
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                        HttpContext.Session.SetString("JWToken", token.ToString());
                        return Ok(new
                        {
                            Message = "Login Successful",
                            token = token,
                            statusCode = StatusCode(201),
                            userId = userId,
                            firstName = firstName,
                            lastName = lastName,
                            email = email != null ? email : "",
                        });
                    }
                    else
                    {
                        return Unauthorized("Invalid username or password");
                    }
                }
                else
                {
                    return Unauthorized("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest();
            }
        }
        private IEnumerable<Claim> GetUserClaims(UserVM user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, user.UserId.ToString());
            claims.Add(_claim);
            _claim = new Claim("USERID", user.UserId.ToString());
            claims.Add(_claim);
            _claim = new Claim("EMAIL", user.Email);
            claims.Add(_claim);
            return claims.AsEnumerable<Claim>();
        }
    }
}
