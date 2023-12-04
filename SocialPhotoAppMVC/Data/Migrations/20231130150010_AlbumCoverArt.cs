using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialPhotoAppMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlbumCoverArt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverArtUrl",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverArtUrl",
                table: "Albums");
        }
    }
}
