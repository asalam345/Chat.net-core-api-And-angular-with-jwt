using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chat_server.Entity;
using chat_server.Entity.interfaces;
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
			return await _genericService.Entry(value);
		}

		[HttpPut]
		public async Task<Result> Put([FromBody] MessageVM value)
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				return await _genericService.Update(value);
			}
			return null;
		}

		[HttpDelete("{id}")]
		public async Task<Result> Delete(long id)
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				return await _genericService.Delete(id);
			}
			return null;
		}
	}
}
