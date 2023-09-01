using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnimeCommentModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_anime_comments_comment_id",
                table: "anime_comments");

            migrationBuilder.DropColumn(
                name: "comment_id",
                table: "anime_comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "comment_id",
                table: "anime_comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_anime_comments_comment_id",
                table: "anime_comments",
                column: "comment_id",
                unique: true);
        }
    }
}
