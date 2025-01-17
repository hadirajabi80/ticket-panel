using Microsoft.EntityFrameworkCore;
using ticket_panel.entities;

namespace ticket_panel.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<User_tichet> User_Tichets { get; set; }
        public DbSet<Admin_ticket> Admin_tichets { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
