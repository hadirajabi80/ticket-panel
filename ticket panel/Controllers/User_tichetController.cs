using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_panel.Data;
using ticket_panel.Dto;
using ticket_panel.entities;

namespace ticket_panel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_tichetController : ControllerBase
    {
        private readonly DataContext _context;
        public User_tichetController(DataContext context) 
        { 
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<User_tichet>>> Get() 
        {
            var user_tichet = await _context.User_Tichets.ToListAsync();
            return Ok(user_tichet);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User_tichet>> Get(int id)
        {
            var user_tichet = await _context.User_Tichets.FindAsync(id);
            if (user_tichet == null)
                return BadRequest("not found");
            return Ok(user_tichet);
        }
        [HttpPost]
        public async Task<ActionResult<User_tichet>> Add(User_ticketAddDto customerAdd)
        {
            var user_tichet = new User_tichet()
            {
                FirstName = customerAdd.FirstName,
                LastName = customerAdd.LastName,
                PhoneNumber = customerAdd.PhoneNumber,
                Title = customerAdd.Title,
                Description = customerAdd.Description,
                File = customerAdd.File,
                Status=true,
            };
            _context.User_Tichets.Add(user_tichet);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult<User_tichet>> Delete(int id) 
        {
            var user_tichet = await _context.User_Tichets.FindAsync(id);
            if (user_tichet == null)
                return BadRequest("not found");
            _context.User_Tichets.Remove(user_tichet);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
