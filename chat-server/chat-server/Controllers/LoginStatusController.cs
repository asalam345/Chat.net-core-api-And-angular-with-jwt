using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.interfaces;
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
