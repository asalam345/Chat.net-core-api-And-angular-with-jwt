using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace chat_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController: ControllerBase
    {
        private IGenericService<MessageVM> _genericService;
        public ChatController(IGenericService<MessageVM> genericService) 
        {
            _genericService = genericService;
        }
		[HttpGet]
		public async Task<Result> Get([FromQuery] MessageVM model = null)
		{
			if (User.Identity.IsAuthenticated )
			{
				UserVM objLoggedInUser = new UserVM();
				var claimsIndentity = HttpContext.User.Identity as ClaimsIdentity;
                var userClaims = claimsIndentity.Claims;

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    foreach (var claim in userClaims)
                    {
                        var cType = claim.Type;
                        var cValue = claim.Value;
                        switch (cType)
                        {
							case "USERID":
								objLoggedInUser.UserId = Convert.ToInt64(cValue);
								break;
							case "EMAIL":
								objLoggedInUser.Email = cValue;
								break;
						}
                    }
                }
				if (objLoggedInUser.UserId == model.SenderId)
					return await _genericService.Get(model);
			}
			return null;
		}


		[HttpPost]
		public async Task<Result> Post([FromBody] MessageVM value)
		{
			HttpContext.Session.SetString("Key", "This is a test message.");
			return await _genericService.Entry(value);
		}

		// PUT api/<GenericController>/5
		[HttpPut]
		public async Task<Result> Put([FromBody] MessageVM value)
		{
			return await _genericService.Update(value);
		}

		// DELETE api/<GenericController>/5
		[HttpDelete("{id}")]
		public async Task<Result> Delete(long id)
		{
			return await _genericService.Delete(id);
		}
	}
}
