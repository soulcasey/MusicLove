using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicLove.Migrations
{
    /// <inheritdoc />
    public partial class YouTubeRename2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YouTubeLink",
                table: "Blogs",
                newName: "YouTube");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YouTube",
                table: "Blogs",
                newName: "YouTubeLink");
        }
    }
}
