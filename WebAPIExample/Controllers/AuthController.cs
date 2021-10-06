using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIExample.Data;
using WebAPIExample.DTOs.User;
using WebAPIExample.Models;

namespace WebAPIExample.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _authRepository;

		public AuthController(IAuthRepository authRepository)
		{
			_authRepository = authRepository;
		}

		[HttpPost("RegisterUser")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserDto user)
		{
			var response = await _authRepository.Register(
				new User { UserName = user.Username}, user.password
				);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<ServiceResponse<int>>> Login(UserDto user)
		{
			var response = await _authRepository.Login(user.Username,user.password);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
