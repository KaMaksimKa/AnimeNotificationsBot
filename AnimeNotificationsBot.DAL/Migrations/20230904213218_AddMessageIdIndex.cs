using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_bot_messages_message_id",
                table: "bot_messages",
                column: "message_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bot_messages_message_id",
                table: "bot_messages");
        }
    }
}
