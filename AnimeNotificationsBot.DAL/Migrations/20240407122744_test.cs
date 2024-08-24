using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_serial_number",
                table: "anime_notifications");

            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "anime_notifications");

            migrationBuilder.AddColumn<string>(
                name: "img_id_from_anime_go",
                table: "animes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "episode_id",
                table: "anime_notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "video",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quality = table.Column<string>(type: "text", nullable: false),
                    media_document_id = table.Column<long>(type: "bigint", nullable: false),
                    video_info_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video", x => x.id);
                    table.ForeignKey(
                        name: "fk_video_video_info_video_info_id",
                        column: x => x.video_info_id,
                        principalTable: "video_info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_episodes_episode_id_from_anime_go",
                table: "episodes",
                column: "episode_id_from_anime_go",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_episode_id",
                table: "anime_notifications",
                columns: new[] { "anime_id", "dubbing_id", "episode_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_episode_id",
                table: "anime_notifications",
                column: "episode_id");

            migrationBuilder.CreateIndex(
                name: "ix_video_video_info_id",
                table: "video",
                column: "video_info_id");

            migrationBuilder.AddForeignKey(
                name: "fk_anime_notifications_episodes_episode_id",
                table: "anime_notifications",
                column: "episode_id",
                principalTable: "episodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_anime_notifications_episodes_episode_id",
                table: "anime_notifications");

            migrationBuilder.DropTable(
                name: "video");

            migrationBuilder.DropIndex(
                name: "ix_episodes_episode_id_from_anime_go",
                table: "episodes");

            migrationBuilder.DropIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_episode_id",
                table: "anime_notifications");

            migrationBuilder.DropIndex(
                name: "ix_anime_notifications_episode_id",
                table: "anime_notifications");

            migrationBuilder.DropColumn(
                name: "img_id_from_anime_go",
                table: "animes");

            migrationBuilder.DropColumn(
                name: "episode_id",
                table: "anime_notifications");

            migrationBuilder.AddColumn<int>(
                name: "serial_number",
                table: "anime_notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_anime_id_dubbing_id_serial_number",
                table: "anime_notifications",
                columns: new[] { "anime_id", "dubbing_id", "serial_number" },
                unique: true);
        }
    }
}
