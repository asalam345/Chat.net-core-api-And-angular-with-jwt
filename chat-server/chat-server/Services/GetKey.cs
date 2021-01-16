using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace chat_server.Services
{
    public static class GetKey
    {
        internal static async Task<UserDetails> GetSecurityKey(HttpContext httpContext)
        {
            int iUserId = 0;
            string username = "";
            string strSecurityKey = "";
            bool isAuthenticated = false;
            string strClientIpAddress = "";

            if (httpContext.User.Identity.Name != null)
            {
                strClientIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                iUserId = Convert.ToInt32(httpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);
                username = httpContext.User.Identities.FirstOrDefault().Name;
                isAuthenticated = httpContext.User.Identities.FirstOrDefault().IsAuthenticated;
                string strKey = (httpContext.GetTokenAsync("access_token").Result) + username;
                strSecurityKey = strKey.Substring(strKey.Length - 32, 32);
            }            
			
			string controllerName = httpContext.Request.RouteValues.Values.ToList()[1].ToString();

            if (controllerName.Equals("Auth"))
            {
                strSecurityKey = "Auth^%$#@!Trn";
            }

            if (controllerName.Equals("Customer"))
            {
                strSecurityKey = "CusTomer^%$#@!Trn";
            }
            

            UserDetails userDetails = new UserDetails
            {
                UserId = iUserId,
                Username = username,
                SecurityKey = strSecurityKey,
                IsAuthenticated = isAuthenticated,
                ClientIPAddress = strClientIpAddress
            };

            return await Task.FromResult(userDetails);
        }
    }
    public class UserDetails
	{
        public long UserId { get; set; }
        public string Username { get; set; }
        public string SecurityKey { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ClientIPAddress { get; set; }
    }
}
