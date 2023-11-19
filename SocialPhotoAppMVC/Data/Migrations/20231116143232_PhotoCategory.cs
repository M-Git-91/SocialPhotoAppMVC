using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialPhotoAppMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotoCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Photos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Photos");
        }
    }
}
