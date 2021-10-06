using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIExample.DTOs;
using WebAPIExample.Models;
using WebAPIExample.Services;

namespace WebAPIExample.Controllers
{
	[Route("[controller]")]
	[ApiController]
	//[Authorize]
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;
		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpGet("GetAll")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
		{
			return Ok(await _characterService.GetAllCharacters());
		}

		[HttpGet("GetSingle/{Id}")]
		public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int Id)
		{
			return Ok(await _characterService.GetSingleCharacter(Id));
		}

		[HttpPost("AddCharacter")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter (AddCharacterDTO newcharacter)
		{
			
			return Ok(await _characterService.AddCharacter(newcharacter));
		}

		[HttpPut("UpdateCharacter")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> UpdateCharacter(UpdateCharacterDTO updatecharacter)
		{
			var response = await _characterService.UpdateCharacter(updatecharacter);
			if(response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}

		[HttpDelete("DeleteCharacter")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> DeleteCharacter(int id)
		{
			var response = await _characterService.DeleteCharacter(id);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}
	}
}
