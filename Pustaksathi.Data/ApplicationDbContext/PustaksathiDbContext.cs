

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
        #region Attribute configuration
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
        #endregion

        // =============================
        // User Configuration ==========
        // =============================
        #region User configuration
        public void UserModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Users>()
                    .HasKey(u => u.UserId);

            builder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithOne()
                .HasForeignKey<Users>(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Users>()
                .HasAlternateKey(u => u.Email);
        }
        #endregion

        // =============================
        // Book Configuration ==========
        // =============================
        #region Book configuration

        public void BookModelBuilder(ModelBuilder builder)
        {
            builder.Entity<BooksDetails>()
                    .HasIndex(b => b.ISBN)
                    .IsUnique();

            builder.Entity<BooksDetails>()
                .HasKey(b => b.BookId);
        }
        #endregion
    }

}
