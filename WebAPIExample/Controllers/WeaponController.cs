using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIExample.DTOs.Weapon;
using WebAPIExample.Services;

namespace WebAPIExample.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class WeaponController : ControllerBase
	{
		private readonly IWeaponService _weaponService;

		public WeaponController(IWeaponService weaponService)
		{
			_weaponService = weaponService;
		}

		[HttpPost("AddWeapon")]
		public async Task<IActionResult> AddWeapon(AddWeaponDto newWeapon)
		{
			return Ok(await _weaponService.AddWeapon(newWeapon));
		}
	}
}
