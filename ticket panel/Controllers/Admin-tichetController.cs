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
    public class Admin_tichetController : ControllerBase
    {
        private readonly DataContext _context;
        public Admin_tichetController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Admin_ticket>>> Get()
        {
            var admin_tichet = await _context.Admin_tichets.ToListAsync();
            return Ok(admin_tichet);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin_ticket>> Get(int id)
        {
            var admin_tichet = await _context.Admin_tichets.FindAsync(id);
            if (admin_tichet == null)
                return BadRequest("not found");
            return Ok(admin_tichet);
        }
        [HttpPost]
        public async Task<ActionResult<Admin_ticket>> Add(Admin_tichetAddDto admin_tichetAdd)
        {
            var admin_tichet = new Admin_ticket()
            {
                Title = admin_tichetAdd.Title,
                Description = admin_tichetAdd.Description,
                UserTicketId = admin_tichetAdd.UserTicketId
            };
            _context.Admin_tichets.Add(admin_tichet);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult<Admin_ticket>> Delete(int id)
        {
            var admin_tichet = await _context.Admin_tichets.FindAsync(id);
            if (admin_tichet == null)
                return BadRequest("not found");
            _context.Admin_tichets.Remove(admin_tichet);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}

