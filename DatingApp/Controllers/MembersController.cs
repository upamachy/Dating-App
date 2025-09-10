using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
    {
    public class MembersController ( AppDbContext context ) : BaseController
        {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers ( )
            {
            var users = await context.Users.ToListAsync ( );
            return Ok (users);
            }
        [HttpGet ("{id}")]
        public async Task<ActionResult<AppUser>> GetMember ( string id )
            {
            var user =await context.Users.FindAsync (id);
            if (user == null) return NotFound ( );
            return Ok (user);
            }
        }
    }
