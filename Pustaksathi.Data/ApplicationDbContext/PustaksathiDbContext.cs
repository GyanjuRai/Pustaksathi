

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Attribute;

namespace Pustaksathi.Data.ApplicationDbContext
{
    public class PustaksathiDbContext : DbContext
    {
        public PustaksathiDbContext(DbContextOptions<PustaksathiDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<BooksDetails> Books { get; set; }
        public DbSet<AttributeItem> AttributeItems { get; set; }
        public DbSet<AttributeCategory> AttributeCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AttributeModelBuilder(modelBuilder);
            UserModelBuilder(modelBuilder);
            BookModelBuilder(modelBuilder);
        }

        // ============================
        // Attribute Configuration ====
        // ============================
        public void AttributeModelBuilder(ModelBuilder builder)
        {

            
            builder.Entity<AttributeCategory>(entity =>
            {
                entity.HasKey(tb => tb.AttributeCategoryId);
                entity.Property(tb => tb.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(tb => tb.AttributeItems)
                    .WithOne(tb => tb.AttributeCategory)
                    .HasForeignKey(tb => tb.AttributeCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<AttributeItem>(entity =>
            {
                entity.HasKey(tb => tb.AttributeItemId);

                entity.Property(tb => tb.ItemName)
                 .IsRequired();

                entity.Property(tb => tb.ItemValue)
                .IsRequired();

                entity.Property(tb => tb.AttributeCategoryId)
                .IsRequired();
            });
        }

        // =============================
        // User Configuration ==========
        // =============================
        public void UserModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Users>()
                    .HasKey(u => u.UserId);

            builder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithOne()
                .HasForeignKey<Users>(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // =============================
        // Book Configuration ==========
        // =============================
        public void BookModelBuilder(ModelBuilder builder)
        {
            builder.Entity<BooksDetails>()
                    .HasIndex(b => b.ISBN)
                    .IsUnique();

            builder.Entity<BooksDetails>()
                .HasKey(b => b.BookId);
        }
    }

}
