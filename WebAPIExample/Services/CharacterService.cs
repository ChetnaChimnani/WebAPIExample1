using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPIExample.Data;
using WebAPIExample.DTOs;
using WebAPIExample.Models;

namespace WebAPIExample.Services
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _HttpContextAccessor;
		public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
		{
			_mapper = mapper;
			_context = context;
			_HttpContextAccessor = httpContextAccessor;
		}

		private int GetUserId() => int.Parse(_HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

		public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newcharacter)
		{
			var serviceresponse = new ServiceResponse<List<GetCharacterDTO>>();
			Character character = _mapper.Map<Character>(newcharacter);
			character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);
			_context.Characters.Add(character);
			await _context.SaveChangesAsync();
			//character.Id = characters.Max(c => c.Id) + 1;
			//characters.Add(character);
			serviceresponse.Data = _context.Characters.
				 Where(c=>c.User.Id == 1)
				.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
			return serviceresponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
		{
			var serviceresponse = new ServiceResponse<List<GetCharacterDTO>>();
			try
			{
				Character character =await _context.Characters.FirstOrDefaultAsync(c=>c.Id == id && c.User.Id ==1);
				if (character != null)
				{
					_context.Characters.Remove(character);
					await _context.SaveChangesAsync();

					serviceresponse.Data = _context.Characters
						.Where(c => c.User.Id == 1)
						.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
				}
				else
				{
					serviceresponse.Success = false;
					serviceresponse.Message = "Character Not Found";
				}
			}
			catch (Exception ex)
			{
				serviceresponse.Success = false;
				serviceresponse.Message = ex.Message;
			}
			return serviceresponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
		{
			var serviceresponse = new ServiceResponse<List<GetCharacterDTO>>();
			var dbCharacters = await _context.Characters.Where(c=>c.User.Id == 1).ToListAsync();
			serviceresponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList(); 
			return serviceresponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> GetSingleCharacter(int id)
		{
			var serviceresponse = new ServiceResponse<GetCharacterDTO>();
			var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == 1);
			serviceresponse.Data =_mapper.Map<GetCharacterDTO>(dbCharacter);
			return serviceresponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
		{
			var serviceresponse = new ServiceResponse<GetCharacterDTO>();
			try
			{
				Character character = await _context.Characters.Include(c=>c.User)
					.FirstOrDefaultAsync(c=>c.Id==updateCharacter.Id);
				if (character.User.Id == 1)
				{
					character.Name = updateCharacter.Name;
					character.HitPoints = updateCharacter.HitPoints;
					character.Strength = updateCharacter.Strength;
					character.Defense = updateCharacter.Defense;
					character.Intelligence = updateCharacter.Intelligence;
					character.Class = updateCharacter.Class;

					await _context.SaveChangesAsync();
					serviceresponse.Data = _mapper.Map<GetCharacterDTO>(character);
				}
				else
				{
					serviceresponse.Success = false;
					serviceresponse.Message = "Character Not Found";
				}
			}
			catch (Exception ex) {
				serviceresponse.Success = false;
				serviceresponse.Message = ex.Message;
			}
			return serviceresponse;
		}
	}
}
