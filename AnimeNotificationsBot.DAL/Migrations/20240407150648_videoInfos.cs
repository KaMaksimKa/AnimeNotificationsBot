using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class videoInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_video_video_info_video_info_id",
                table: "video");

            migrationBuilder.DropForeignKey(
                name: "fk_video_info_dubbing_dubbing_id",
                table: "video_info");

            migrationBuilder.DropForeignKey(
                name: "fk_video_info_episodes_episode_id",
                table: "video_info");

            migrationBuilder.DropForeignKey(
                name: "fk_video_info_video_providers_video_provider_id",
                table: "video_info");

            migrationBuilder.DropPrimaryKey(
                name: "pk_video_info",
                table: "video_info");

            migrationBuilder.RenameTable(
                name: "video_info",
                newName: "video_infos");

            migrationBuilder.RenameIndex(
                name: "ix_video_info_video_provider_id",
                table: "video_infos",
                newName: "ix_video_infos_video_provider_id");

            migrationBuilder.RenameIndex(
                name: "ix_video_info_episode_id",
                table: "video_infos",
                newName: "ix_video_infos_episode_id");

            migrationBuilder.RenameIndex(
                name: "ix_video_info_dubbing_id",
                table: "video_infos",
                newName: "ix_video_infos_dubbing_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_video_infos",
                table: "video_infos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_video_infos_video_info_id",
                table: "video",
                column: "video_info_id",
                principalTable: "video_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_video_infos_dubbing_dubbing_id",
                table: "video_infos",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_infos_episodes_episode_id",
                table: "video_infos",
                column: "episode_id",
                principalTable: "episodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_infos_video_providers_video_provider_id",
                table: "video_infos",
                column: "video_provider_id",
                principalTable: "video_providers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_video_video_infos_video_info_id",
                table: "video");

            migrationBuilder.DropForeignKey(
                name: "fk_video_infos_dubbing_dubbing_id",
                table: "video_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_video_infos_episodes_episode_id",
                table: "video_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_video_infos_video_providers_video_provider_id",
                table: "video_infos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_video_infos",
                table: "video_infos");

            migrationBuilder.RenameTable(
                name: "video_infos",
                newName: "video_info");

            migrationBuilder.RenameIndex(
                name: "ix_video_infos_video_provider_id",
                table: "video_info",
                newName: "ix_video_info_video_provider_id");

            migrationBuilder.RenameIndex(
                name: "ix_video_infos_episode_id",
                table: "video_info",
                newName: "ix_video_info_episode_id");

            migrationBuilder.RenameIndex(
                name: "ix_video_infos_dubbing_id",
                table: "video_info",
                newName: "ix_video_info_dubbing_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_video_info",
                table: "video_info",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_video_info_video_info_id",
                table: "video",
                column: "video_info_id",
                principalTable: "video_info",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_video_info_dubbing_dubbing_id",
                table: "video_info",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_info_episodes_episode_id",
                table: "video_info",
                column: "episode_id",
                principalTable: "episodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_info_video_providers_video_provider_id",
                table: "video_info",
                column: "video_provider_id",
                principalTable: "video_providers",
                principalColumn: "id");
        }
    }
}
