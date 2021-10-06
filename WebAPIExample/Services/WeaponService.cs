using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExample.Data;
using WebAPIExample.DTOs;
using WebAPIExample.DTOs.Weapon;
using WebAPIExample.Models;

namespace WebAPIExample.Services
{
	public class WeaponService : IWeaponService
	{
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;

		public WeaponService(DataContext context,IHttpContextAccessor httpContextAccessor,IMapper mapper)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
		}
		public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDto newWeapon)
		{
			var serviceresponse = new ServiceResponse<GetCharacterDTO>();
			try
			{
				Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == 1);
				if(character == null)
				{
					serviceresponse.Success = false;
					serviceresponse.Message = "Character Not Found";
				}
				Weapon weapon = new Weapon
				{
					Name = newWeapon.Name,
					Damage = newWeapon.Damage,
					Character = character
				};
				await _context.Weapons.AddAsync(weapon);
				await _context.SaveChangesAsync();
				//character.Weapon = weapon;
				serviceresponse.Data = _mapper.Map<GetCharacterDTO>(character);
				return serviceresponse;
			}
			catch(Exception ex)
			{
				serviceresponse.Success = false;
				serviceresponse.Message = ex.Message;
			}
			return serviceresponse;
		}
	}
}
