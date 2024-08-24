using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class videos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_video_video_infos_video_info_id",
                table: "video");

            migrationBuilder.DropPrimaryKey(
                name: "pk_video",
                table: "video");

            migrationBuilder.RenameTable(
                name: "video",
                newName: "videos");

            migrationBuilder.RenameIndex(
                name: "ix_video_video_info_id",
                table: "videos",
                newName: "ix_videos_video_info_id");

            migrationBuilder.AlterColumn<string>(
                name: "video_player_link",
                table: "video_infos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "media_document_id",
                table: "videos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "manifest_link",
                table: "videos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_videos",
                table: "videos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_videos_video_infos_video_info_id",
                table: "videos",
                column: "video_info_id",
                principalTable: "video_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_videos_video_infos_video_info_id",
                table: "videos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_videos",
                table: "videos");

            migrationBuilder.DropColumn(
                name: "manifest_link",
                table: "videos");

            migrationBuilder.RenameTable(
                name: "videos",
                newName: "video");

            migrationBuilder.RenameIndex(
                name: "ix_videos_video_info_id",
                table: "video",
                newName: "ix_video_video_info_id");

            migrationBuilder.AlterColumn<string>(
                name: "video_player_link",
                table: "video_infos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "media_document_id",
                table: "video",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_video",
                table: "video",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_video_video_infos_video_info_id",
                table: "video",
                column: "video_info_id",
                principalTable: "video_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
