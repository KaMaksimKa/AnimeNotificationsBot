using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addepisode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anime_dubbing1");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropColumn(
                name: "img_id_from_anime_go",
                table: "animes");

            migrationBuilder.RenameColumn(
                name: "id_from_anime_go",
                table: "animes",
                newName: "anime_id_from_anime_go");

            migrationBuilder.RenameIndex(
                name: "ix_animes_id_from_anime_go",
                table: "animes",
                newName: "ix_animes_anime_id_from_anime_go");

            migrationBuilder.CreateTable(
                name: "episodes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    episode_id_from_anime_go = table.Column<long>(type: "bigint", nullable: true),
                    number = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    released = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    anime_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_episodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_episodes_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_providers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "video_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    video_player_link = table.Column<string>(type: "text", nullable: true),
                    episode_id = table.Column<long>(type: "bigint", nullable: true),
                    dubbing_id = table.Column<long>(type: "bigint", nullable: true),
                    video_provider_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_video_info_dubbing_dubbing_id",
                        column: x => x.dubbing_id,
                        principalTable: "dubbing",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_video_info_episodes_episode_id",
                        column: x => x.episode_id,
                        principalTable: "episodes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_video_info_video_providers_video_provider_id",
                        column: x => x.video_provider_id,
                        principalTable: "video_providers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_episodes_anime_id",
                table: "episodes",
                column: "anime_id");

            migrationBuilder.CreateIndex(
                name: "ix_video_info_dubbing_id",
                table: "video_info",
                column: "dubbing_id");

            migrationBuilder.CreateIndex(
                name: "ix_video_info_episode_id",
                table: "video_info",
                column: "episode_id");

            migrationBuilder.CreateIndex(
                name: "ix_video_info_video_provider_id",
                table: "video_info",
                column: "video_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_video_providers_name",
                table: "video_providers",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "video_info");

            migrationBuilder.DropTable(
                name: "episodes");

            migrationBuilder.DropTable(
                name: "video_providers");

            migrationBuilder.RenameColumn(
                name: "anime_id_from_anime_go",
                table: "animes",
                newName: "id_from_anime_go");

            migrationBuilder.RenameIndex(
                name: "ix_animes_anime_id_from_anime_go",
                table: "animes",
                newName: "ix_animes_id_from_anime_go");

            migrationBuilder.AddColumn<string>(
                name: "img_id_from_anime_go",
                table: "animes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "anime_dubbing1",
                columns: table => new
                {
                    dubbing_from_first_episode_id = table.Column<long>(type: "bigint", nullable: false),
                    first_episode_animes_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_dubbing1", x => new { x.dubbing_from_first_episode_id, x.first_episode_animes_id });
                    table.ForeignKey(
                        name: "fk_anime_dubbing1_animes_first_episode_animes_id",
                        column: x => x.first_episode_animes_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_dubbing1_dubbing_dubbing_from_first_episode_id",
                        column: x => x.dubbing_from_first_episode_id,
                        principalTable: "dubbing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    anime_id = table.Column<long>(type: "bigint", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_anime_dubbing1_first_episode_animes_id",
                table: "anime_dubbing1",
                column: "first_episode_animes_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_anime_id",
                table: "images",
                column: "anime_id");
        }
    }
}
