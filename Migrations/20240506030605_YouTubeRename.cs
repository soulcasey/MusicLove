using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicLove.Migrations
{
    /// <inheritdoc />
    public partial class YouTubeRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeLink",
                table: "Blogs",
                newName: "YouTubeLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YouTubeLink",
                table: "Blogs",
                newName: "YoutubeLink");
        }
    }
}
