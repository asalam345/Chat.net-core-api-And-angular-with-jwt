using Microsoft.AspNetCore.Mvc;
using chat_server.Entity;
using chat_server.Entity.interfaces;
using Newtonsoft.Json;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginStatusController : GenericController<tblLogedinStatus>
    {
        private IGenericService<tblLogedinStatus> _genericService;
        public LoginStatusController(IGenericService<tblLogedinStatus> genericService) : base(genericService)
        {
            _genericService = genericService;
        }
    }
}
