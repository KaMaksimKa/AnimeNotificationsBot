using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToAnimeNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_anime_notifications_anime_id",
                table: "anime_notifications");

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_serial_number",
                table: "anime_notifications",
                columns: new[] { "anime_id", "dubbing_id", "serial_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_serial_number",
                table: "anime_notifications");

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_anime_id",
                table: "anime_notifications",
                column: "anime_id");
        }
    }
}
