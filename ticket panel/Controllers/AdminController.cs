using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sanay.NetFlow.Library.Class;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ticket_panel.Data;
using ticket_panel.Dto;
using ticket_panel.entities;

namespace ticket_panel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AdminController(DataContext context
                               ,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<Admin>> Login(LoginDto loginDto)
        {
            var admin = await _context.Admins.Where(x => x.Username == loginDto.Username).FirstOrDefaultAsync();

            var password = Cryption.Encrypt(loginDto.Password, "HZ123RS456");
            if (admin == null && admin.PasswordHash != password)
            {
                return BadRequest("username or password is not correct");
            }
            string token = CreateToken(admin);
            return Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult<Admin>> Add(AdminAddDto userAdd)
        {
            var admin = new Admin()
            {
                FirstName = userAdd.FirstName,
                LastName = userAdd.LastName,
                Username = userAdd.Username,
                PasswordHash = Cryption.Encrypt(userAdd.Password, "HZ123RS456")
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult<Admin>> Delete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return BadRequest("not found");
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtToken:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
