using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagHref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "img_href",
                table: "animes",
                newName: "img_id_from_anime_go");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "img_id_from_anime_go",
                table: "animes",
                newName: "img_href");
        }
    }
}
