using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chat_server.Entity;
using chat_server.Entity.interfaces;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GenericController<T> : ControllerBase where T:class
	{
		private IGenericService<T> _genericService;
		public GenericController(IGenericService<T> genericService)
		{
			_genericService = genericService;
		}
		[HttpGet]
		public async Task<Result> Get([FromQuery] T model = null)
		{
			if (User.Identity.IsAuthenticated)
			{
				return await _genericService.Get(model);
			}
			
				return null;
			
		}
		
		[HttpPost]
		public async Task<Result> Post([FromBody] T value)
		{

			if (User.Identity.IsAuthenticated)
			{
				return await _genericService.Entry(value);
			}
			return null;
		}

		[HttpPut]
		public async Task<Result> Put([FromBody] T value)
		{
			if (User.Identity.IsAuthenticated)
			{
				return await _genericService.Update(value);
			}
			return null;
		}

		[HttpDelete("{id}")]
		public async Task<Result> Delete(long id)
		{
			if (User.Identity.IsAuthenticated)
			{
				return await _genericService.Delete(id);
			}
			return null;
		}
	}
}
