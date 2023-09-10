using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CheckOnChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscription_animes_anime_id",
                table: "anime_subscription");

            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscription_dubbing_dubbing_id",
                table: "anime_subscription");

            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscription_users_user_id",
                table: "anime_subscription");

            migrationBuilder.DropForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages");

            migrationBuilder.DropIndex(
                name: "ix_bot_messages_user_id",
                table: "bot_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_anime_subscription",
                table: "anime_subscription");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "studios");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "mpaa_rates");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "genres");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "dubbing");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "bot_messages");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "anime_types");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "anime_statuses");

            migrationBuilder.RenameTable(
                name: "anime_subscription",
                newName: "anime_subscriptions");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscription_user_id",
                table: "anime_subscriptions",
                newName: "ix_anime_subscriptions_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscription_dubbing_id",
                table: "anime_subscriptions",
                newName: "ix_anime_subscriptions_dubbing_id");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscription_anime_id",
                table: "anime_subscriptions",
                newName: "ix_anime_subscriptions_anime_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_anime_subscriptions",
                table: "anime_subscriptions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscriptions_animes_anime_id",
                table: "anime_subscriptions",
                column: "anime_id",
                principalTable: "animes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscriptions_dubbing_dubbing_id",
                table: "anime_subscriptions",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscriptions_users_user_id",
                table: "anime_subscriptions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscriptions_animes_anime_id",
                table: "anime_subscriptions");

            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscriptions_dubbing_dubbing_id",
                table: "anime_subscriptions");

            migrationBuilder.DropForeignKey(
                name: "fk_anime_subscriptions_users_user_id",
                table: "anime_subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_anime_subscriptions",
                table: "anime_subscriptions");

            migrationBuilder.RenameTable(
                name: "anime_subscriptions",
                newName: "anime_subscription");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscriptions_user_id",
                table: "anime_subscription",
                newName: "ix_anime_subscription_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscriptions_dubbing_id",
                table: "anime_subscription",
                newName: "ix_anime_subscription_dubbing_id");

            migrationBuilder.RenameIndex(
                name: "ix_anime_subscriptions_anime_id",
                table: "anime_subscription",
                newName: "ix_anime_subscription_anime_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "studios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "mpaa_rates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "genres",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "dubbing",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "bot_messages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "anime_types",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                table: "anime_statuses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "pk_anime_subscription",
                table: "anime_subscription",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_bot_messages_user_id",
                table: "bot_messages",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscription_animes_anime_id",
                table: "anime_subscription",
                column: "anime_id",
                principalTable: "animes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscription_dubbing_dubbing_id",
                table: "anime_subscription",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_anime_subscription_users_user_id",
                table: "anime_subscription",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bot_messages_users_user_id",
                table: "bot_messages",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
