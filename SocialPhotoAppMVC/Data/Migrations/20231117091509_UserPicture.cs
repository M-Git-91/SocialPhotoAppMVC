using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialPhotoAppMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureURL",
                table: "AspNetUsers");
        }
    }
}
