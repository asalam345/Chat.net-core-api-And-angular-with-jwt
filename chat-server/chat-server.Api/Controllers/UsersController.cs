using Microsoft.AspNetCore.Mvc;
using chat_server.Entity;
using chat_server.Entity.interfaces;


namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : GenericController<UserVM>
    {
		private IGenericService<UserVM> _genericService;
        public UsersController(IGenericService<UserVM> genericService) :base(genericService)
		{
			_genericService = genericService;
		}
		[HttpGet("welcome")]
		public string Welcome()
		{
			return "Welcome to our chat application";
		}
    }
}
