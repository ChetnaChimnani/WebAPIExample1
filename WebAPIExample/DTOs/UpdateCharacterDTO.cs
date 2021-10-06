﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExample.Models;

namespace WebAPIExample.DTOs
{
	public class UpdateCharacterDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = "Chetna";
		public int HitPoints { get; set; } = 100;
		public int Strength { get; set; } = 10;
		public int Defense { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RpgClass Class { get; set; } = RpgClass.Knight;

		//Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
	}
}
