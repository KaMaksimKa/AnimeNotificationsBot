using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages");

            migrationBuilder.DropColumn(
                name: "command_group",
                table: "bot_messages");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "bot_messages");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "bot_messages",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "bot_message_group_id",
                table: "bot_messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "bot_message_groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bot_message_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_bot_message_groups_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bot_messages_bot_message_group_id",
                table: "bot_messages",
                column: "bot_message_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_bot_message_groups_user_id",
                table: "bot_message_groups",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_bot_messages_bot_message_groups_bot_message_group_id",
                table: "bot_messages",
                column: "bot_message_group_id",
                principalTable: "bot_message_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bot_messages_bot_message_groups_bot_message_group_id",
                table: "bot_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages");

            migrationBuilder.DropTable(
                name: "bot_message_groups");

            migrationBuilder.DropIndex(
                name: "ix_bot_messages_bot_message_group_id",
                table: "bot_messages");

            migrationBuilder.DropColumn(
                name: "bot_message_group_id",
                table: "bot_messages");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "bot_messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "command_group",
                table: "bot_messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "bot_messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
