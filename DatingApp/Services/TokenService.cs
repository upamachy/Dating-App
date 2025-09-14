using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Services
    {
    public class TokenService (IConfiguration config): ITokenService
        {
        public string CreateToken ( AppUser user )
            {
            var tokenkey = config["TokenKey"] ?? throw new Exception ("Can not get token key");
            if (tokenkey.Length < 64) { throw new Exception ("TokenKey must be at least 64 characters long"); }
            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (tokenkey));

            var claim = new List<Claim>
                {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                };

            var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor { 
               Subject = new ClaimsIdentity (claim),
               Expires = DateTime.Now.AddDays (7),
               SigningCredentials = creds
               };
            var tokenHandler = new JwtSecurityTokenHandler ( );
            var token = tokenHandler.CreateToken (tokenDescriptor);
            return tokenHandler.WriteToken (token);
            }
    }
}
