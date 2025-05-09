

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Application.Members;
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
        public DbSet<AttributeItem> AttributeItems { get; set; }
        public DbSet<AttributeCategory> AttributeCategories { get; set; }
        public DbSet<BooksDetails> Books { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<WhiteList> WhiteLists { get; set; }
        public DbSet<WhiteListItems> WhiteListItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<TimeDiscount> TimeDiscounts { get; set; }
        public DbSet<Annoucement> Annoucements { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AttributeModelBuilder(modelBuilder);
            UserModelBuilder(modelBuilder);
            BookModelBuilder(modelBuilder);
            OrderModelBuilder(modelBuilder);
            CartModelBuilder(modelBuilder);
            WhiteListModelBuilder(modelBuilder);
            TimeDiscountModelBuilder(modelBuilder);
            AnnoucementModelBuilder(modelBuilder);
            ReviewModelBuilder(modelBuilder);
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
                 .IsRequired()
                 .HasMaxLength(100);

                entity.Property(tb => tb.ItemValue)
                .IsRequired()
                .HasMaxLength(100);

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
            builder.Entity<Users>(
                tb =>
                {
                    tb.HasKey(u => u.UserId);

                    tb.HasOne(u => u.Role)
                    .WithOne()
                    .HasForeignKey<Users>(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasAlternateKey(u => u.Email);

                });    
        }
        #endregion

        // =============================
        // Book Configuration ==========
        // =============================
        #region Book configuration

        public void BookModelBuilder(ModelBuilder builder)
        {
            builder.Entity<BooksDetails>(
                tb =>
                {
                    tb.HasIndex(b => b.ISBN)
                    .IsUnique();
                });

            builder.Entity<BooksDetails>(
                tb =>
                {
                    tb.HasKey(b => b.BookId);
                });
        }
        #endregion

        // =============================
        // Order Configuration =========
        // =============================
        #region Order configuration
        public void OrderModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Orders>(
                tb =>
                {
                    tb.HasKey(o => o.OrderId);

                    tb.Property(tb => tb.Status)
                    .HasMaxLength(50)
                    .IsRequired();
                    tb.Property(tb => tb.ClaimCode)
                    .HasMaxLength(50)
                    .IsRequired();

                    tb.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasQueryFilter(tb => !tb.IsCancelled);
                });

            builder.Entity<OrderItems>(
                tb =>
                {
                    tb.HasKey(tb => tb.OrderItemId);

                    tb.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasOne(oi => oi.Book)
                    .WithMany(b => b.OrderItems)
                    .HasForeignKey(oi => oi.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
                });
        }
        #endregion

        // ============================
        // Cart Configuration =========
        // ============================
        #region Cart configuration
        public void CartModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Cart>(
                tb =>
                {
                    tb.HasKey(tb => tb.CartId);

                    tb.HasOne(tb => tb.User)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                });
            builder.Entity<CartItems>(
                tb =>
                {
                    tb.HasKey(tb => tb.CartItemId);

                    tb.HasOne(tb => tb.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(tb => tb.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasOne(tb => tb.Book)
                    .WithMany(b => b.CartItems)
                    .HasForeignKey(tb => tb.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
                });
        }
        #endregion

        // ============================
        // WhiteList Configuration ====
        // ============================
        #region WhiteList configuration
        public void WhiteListModelBuilder(ModelBuilder builder)
        {
            builder.Entity<WhiteList>(
                tb =>
                {
                    tb.HasKey(tb => tb.WhiteListId);
                    
                    tb.HasOne(tb => tb.users)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                });

            builder.Entity<WhiteListItems>(
                tb =>
                {
                    tb.HasKey(tb => tb.WhiteListItemId);

                    tb.HasOne(tb => tb.WhiteList)
                    .WithMany(wl => wl.WhiteListItems)
                    .HasForeignKey(tb => tb.WhiteListId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasOne(tb => tb.books)
                    .WithMany(b => b.WhiteListItems)
                    .HasForeignKey(tb => tb.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
                });
        }
        #endregion

        // ============================
        // TimeDiscount Configuration ==
        // ============================
        #region TimeDiscount configuration
        public void TimeDiscountModelBuilder(ModelBuilder builder)
        {
            builder.Entity<TimeDiscount>(
                tb =>
                {
                    tb.HasKey(tb => tb.DiscountId);

                    tb.HasOne(tb => tb.Book)
                    .WithMany(b => b.TimeDiscounts)
                    .HasForeignKey(tb => tb.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                });
        }
        #endregion

        // ============================
        // Annoucement Configuration ===
        // ============================
        #region Annoucement configuration
        public void AnnoucementModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Annoucement>(
                tb =>
                {
                    tb.HasKey(tb => tb.AnnoucementId);
                });
        }
        #endregion

        // ============================
        // Review Configuration =======
        // ============================
        #region Review configuration
        public void ReviewModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Review>(
                tb =>
                {
                    tb.HasKey(tb => tb.ReviewId);

                    tb.HasOne(tb => tb.User)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasOne(tb => tb.Book)
                    .WithMany(tb => tb.Reviews)
                    .HasForeignKey(tb => tb.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                });
        }
        #endregion
    }

}
