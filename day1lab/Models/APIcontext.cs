using day1lab.Migrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace day1lab.Models
{
    public class APIcontext : IdentityDbContext<ApplicationUser>
    {
        public APIcontext()
        {
            
        }

        public APIcontext(DbContextOptions options) : base(options)
        {
            
        }
       
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
