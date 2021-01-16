using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
			return await _genericService.Get(model);
		}

		
		[HttpPost]
		public async Task<Result> Post([FromBody] T value)
		{
			return await _genericService.Entry(value);
		}

		// PUT api/<GenericController>/5
		[HttpPut]
		public async Task<Result> Put([FromBody] T value)
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
