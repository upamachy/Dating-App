using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
    {
    public class AccountController(AppDbContext context) : BaseController
        {
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
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

            return user;
            }
        private async Task<bool> EmailExist(string email )
            {
             return await context.Users.AnyAsync(x=> x.Email.ToLower() == email.ToLower() );
            }
        }
    }
