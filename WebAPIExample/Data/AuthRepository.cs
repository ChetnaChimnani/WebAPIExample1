
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPIExample.Models;

namespace WebAPIExample.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly DataContext _context;
		private readonly IConfiguration _configuration;

		public AuthRepository(DataContext context,IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		public async Task<ServiceResponse<string>> Login(string username, string password)
		{
			var response = new ServiceResponse<string>();
			var user = await _context.Users.FirstOrDefaultAsync(c => c.UserName.ToLower() == username.ToLower());
			if(user==null)
			{
				response.Success = false;
				response.Message = "User not found";
			}
			else if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
			{
				response.Success = false;
				response.Message = "Password Wrong";
			}
			else
			{
				response.Data = CreateToken(user);
			}
			return response;
		}

		public async Task<ServiceResponse<int>> Register(User user, string password)
		{
			ServiceResponse<int> response = new ServiceResponse<int>();
			if (await UserExists(user.UserName))
			{
				response.Success = false;
				response.Message = "User Already Exists";
				return response;
			}

			CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordsalt);
			user.PasswordHash = passwordhash;
			user.PasswordSalt = passwordsalt;	
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			
			response.Data = user.Id;
			return response;
		}

		public async Task<bool> UserExists(string username)
		{
			if (await _context.Users.AnyAsync(c => c.UserName.ToLower() == username.ToLower()))
			{
				return true;
			}
			return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordhash,out byte[] passwordsalt)
		{
			using(var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordsalt = hmac.Key;
				passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private bool VerifyPassword(string password, byte[] passwordhash, byte[] passwordsalt)
		{
			using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
			{
				var computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				for (int i= 0; i < computehash.Length; i++)
				{
					if (computehash[i] != passwordhash[i])
					{
						return false;
					}
				}
				return true;
			}
		}
		private string CreateToken(User user)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new Claim(ClaimTypes.Name,user.UserName)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = System.DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
