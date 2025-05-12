

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.DataModels;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Attribute;

namespace Pustaksathi.Data.ApplicationDbContext
{
    public class PustaksathiDbContext : DbContext
    {
        public PustaksathiDbContext(DbContextOptions<PustaksathiDbContext> options) : base(options)
        {
        }

        public DbSet<UserDto> Users { get; set; }
        public DbSet<AttributeItemDto> AttributeItems { get; set; }
        public DbSet<AttributeCategoryDto> AttributeCategories { get; set; }
        public DbSet<BooksDetailsDto> Books { get; set; }
        public DbSet<OrdersDto> Orders { get; set; }
        public DbSet<OrderItemsDto> OrderItems { get; set; }
        public DbSet<WhiteListDto> WhiteLists { get; set; }
        public DbSet<WhiteListItemsDto> WhiteListItems { get; set; }
        public DbSet<CartDto> Carts { get; set; }
        public DbSet<CartItemsDto> CartItems { get; set; }
        public DbSet<TimeDiscountDto> TimeDiscounts { get; set; }
        public DbSet<AnnoucementDto> Annoucements { get; set; }
        public DbSet<ReviewDto> Reviews { get; set; }

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

            
            builder.Entity<AttributeCategoryDto>(entity =>
            {
                entity.HasKey(tb => tb.AttributeCategoryId);
                entity.Property(tb => tb.AttributeCategoryId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(tb => tb.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(tb => tb.AttributeItems)
                    .WithOne(tb => tb.AttributeCategory)
                    .HasForeignKey(tb => tb.AttributeCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<AttributeItemDto>(entity =>
            {
                entity.HasKey(tb => tb.AttributeItemId);
                entity.Property(tb => tb.AttributeItemId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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
            builder.Entity<UserDto>(
                tb =>
                {
                    tb.HasKey(u => u.UserId);
                    tb.Property(u => u.UserId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                    tb.HasOne(u => u.Role)
                    .WithOne()
                    .HasForeignKey<UserDto>(u => u.RoleId)
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
            builder.Entity<BooksDetailsDto>(
                tb =>
                {
                    tb.HasIndex(b => b.ISBN)
                    .IsUnique();
                });

            builder.Entity<BooksDetailsDto>(
                tb =>
                {
                    tb.HasKey(b => b.BookId);
                    tb.Property(b => b.BookId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                });
        }
        #endregion

        // =============================
        // Order Configuration =========
        // =============================
        #region Order configuration
        public void OrderModelBuilder(ModelBuilder builder)
        {
            builder.Entity<OrdersDto>(
                tb =>
                {
                    tb.HasKey(o => o.OrderId);
                    tb.Property(o => o.OrderId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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

                });

            builder.Entity<OrderItemsDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.OrderItemId);
                    tb.Property(tb => tb.OrderItemId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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
            builder.Entity<CartDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.CartId);
                    tb.Property(tb => tb.CartId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                    tb.HasOne(tb => tb.User)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                });
            builder.Entity<CartItemsDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.CartItemId);
                    tb.Property(tb => tb.CartItemId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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
            builder.Entity<WhiteListDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.WhiteListId);
                    tb.Property(tb => tb.WhiteListId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();


                    tb.HasOne(tb => tb.users)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                });

            builder.Entity<WhiteListItemsDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.WhiteListItemId);
                    tb.Property(tb => tb.WhiteListItemId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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
            builder.Entity<TimeDiscountDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.DiscountId);
                    tb.Property(tb => tb.DiscountId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

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
            builder.Entity<AnnoucementDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.AnnoucementId);
                    tb.Property(tb => tb.AnnoucementId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                });
        }
        #endregion

        // ============================
        // Review Configuration =======
        // ============================
        #region Review configuration
        public void ReviewModelBuilder(ModelBuilder builder)
        {
            builder.Entity<ReviewDto>(
                tb =>
                {
                    tb.HasKey(tb => tb.ReviewId);
                    tb.Property(tb => tb.ReviewId)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                    tb.HasOne(tb => tb.User)
                    .WithMany()
                    .HasForeignKey(tb => tb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                    tb.HasOne(tb => tb.Book)
                    .WithMany()
                    .HasForeignKey(tb => tb.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                });
        }
        #endregion
    }

}
