using demodoan1.Models;
using Microsoft.EntityFrameworkCore;

namespace demodoan1.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //public DbSet<Role> Role {  get; set; }
        //public DbSet<User> User { get; set; }
    }
}
