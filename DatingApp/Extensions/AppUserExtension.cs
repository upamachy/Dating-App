using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;

namespace DatingApp.Extensions
    {
    public static class AppUserExtension
        {
        public static UserDto ToDto(this AppUser user, ITokenService token)
            {
            return new UserDto
                {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token.CreateToken(user)
                };
            }
        }
    }
