using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustaksathi.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookImagesecondMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "DiscountPrice",
                table: "TimeDiscounts",
                newName: "DiscountPercent");

            migrationBuilder.RenameColumn(
                name: "DiscountPrice",
                table: "Books",
                newName: "DiscountPercent");

            migrationBuilder.AddColumn<string>(
                name: "BookImage",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BookImage",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "TimeDiscounts",
                newName: "DiscountPrice");

            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "Books",
                newName: "DiscountPrice");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
