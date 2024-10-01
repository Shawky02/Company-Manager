using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entities
{
    public class MyContext : IdentityDbContext<IdentityUser>
    {
        public MyContext(DbContextOptions options) :base(options){ }
        
            
        
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Department)        
                .WithMany()                        
                .HasForeignKey(e => e.DeptID)      
                .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}
