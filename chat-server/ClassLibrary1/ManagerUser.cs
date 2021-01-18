using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace chat-server.Manager
{
	public class ManagerUser
	{
        async Task loginDetails(UserVM model)
        {
            try
            {
                string email = "", userId = "", firstName = "", lastName = "";
                Result result = await _genericService.Get(model);
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
                            //new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(email) ? email : ""),
                            new Claim("EMAIL", !string.IsNullOrEmpty(email) ? email : "")
                    };
                        string tokenValue = _config.GetSection("AppSettings:Token").Value;
                        byte[] tokenByte = Encoding.UTF8.GetBytes(tokenValue);
                        var key = new SymmetricSecurityKey(tokenByte);

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                        //var tokenDescriptor = new SecurityTokenDescriptor
                        //{
                        //    Subject = new ClaimsIdentity(claims),
                        //    Expires = DateTime.Now.AddMinutes(1),
                        //    SigningCredentials = creds
                        //};
                        var JWToken = new JwtSecurityToken(
                             issuer: "https://localhost:44319/",
                             audience: "https://localhost:44319/",
                             claims: GetUserClaims(userData),
                             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                             expires: new DateTimeOffset(DateTime.Now.AddMinutes(1)).DateTime,
                             //Using HS256 Algorithm to encrypt Token - JRozario
                             signingCredentials: creds
                         );
                        var tokenHandler = new JwtSecurityTokenHandler();
                        //var token = tokenHandler.CreateToken(tokenDescriptor);
                        var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                        //var userDetails = await GetKey.GetSecurityKey(HttpContext);
                        HttpContext.Session.SetString("JWToken", token.ToString());
                        return Ok(new
                        {
                            Message = "Login Successful",
                            token = token,//tokenHandler.WriteToken(token),
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

            //if (user.WRITE_ACCESS != "")
            //{
            //    _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
            //    claims.Add(_claim);
            //}
            return claims.AsEnumerable<Claim>();
        }
    }
}
