

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Account;

namespace Pustaksathi.Data.ApplicationDbContext
{
    public class PustaksathiDbContext : DbContext
    {
        public PustaksathiDbContext(DbContextOptions<PustaksathiDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Books> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring them in different place or making all them here is good
            modelBuilder.Entity<Users>()
                    .HasKey(u => u.UserId);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithOne()
                .HasForeignKey<Users>(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Books>()
                    .HasIndex(b => b.ISBN)
                    .IsUnique();
        }
    }
   
}
