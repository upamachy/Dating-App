using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
    {
    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseController
        {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
            {
            if (await EmailExist (registerDto.Email)) { return BadRequest("Email Taken"); }
            using var hmac = new HMACSHA512 ( );
            var user = new AppUser
                {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash (Encoding.UTF8.GetBytes (registerDto.Password)),
                PasswordSalt = hmac.Key
                };
            context.Users.Add (user); // tracking
            await context.SaveChangesAsync ( );// commit/save

            return new UserDto
                {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken (user)
                };
            }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            if (user == null) { return Unauthorized("Invalid Email"); }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
                {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
                }
            return new UserDto
                {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
                };
            }
        private async Task<bool> EmailExist(string email )
            {
             return await context.Users.AnyAsync(x=> x.Email.ToLower() == email.ToLower() );
            }
        }
    }
