using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExample.DTOs;
using WebAPIExample.DTOs.Weapon;
using WebAPIExample.Models;

namespace WebAPIExample.Services
{
	public interface IWeaponService
	{
		Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDto newWeapon);
	}
}
