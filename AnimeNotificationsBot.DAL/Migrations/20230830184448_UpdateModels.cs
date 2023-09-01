using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_anime_notifications_dubbing_dubbing_id",
                table: "anime_notifications");

            migrationBuilder.DropTable(
                name: "anime_user");

            migrationBuilder.RenameColumn(
                name: "is_remove",
                table: "users",
                newName: "is_removed");

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

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "genres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AlterColumn<long>(
                name: "dubbing_id",
                table: "anime_notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "genres_id",
                table: "anime_genre",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "anime_subscription",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    anime_id = table.Column<long>(type: "bigint", nullable: false),
                    dubbing_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_subscription", x => x.id);
                    table.ForeignKey(
                        name: "fk_anime_subscription_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_subscription_dubbing_dubbing_id",
                        column: x => x.dubbing_id,
                        principalTable: "dubbing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_subscription_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_anime_subscription_anime_id",
                table: "anime_subscription",
                column: "anime_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_subscription_dubbing_id",
                table: "anime_subscription",
                column: "dubbing_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_subscription_user_id",
                table: "anime_subscription",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_anime_notifications_dubbing_dubbing_id",
                table: "anime_notifications",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_anime_notifications_dubbing_dubbing_id",
                table: "anime_notifications");

            migrationBuilder.DropTable(
                name: "anime_subscription");

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
                name: "is_removed",
                table: "anime_types");

            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "anime_statuses");

            migrationBuilder.RenameColumn(
                name: "is_removed",
                table: "users",
                newName: "is_remove");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "genres",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "dubbing_id",
                table: "anime_notifications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "genres_id",
                table: "anime_genre",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "anime_user",
                columns: table => new
                {
                    animes_id = table.Column<long>(type: "bigint", nullable: false),
                    users_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_user", x => new { x.animes_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_anime_user_animes_animes_id",
                        column: x => x.animes_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_user_users_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_anime_user_users_id",
                table: "anime_user",
                column: "users_id");

            migrationBuilder.AddForeignKey(
                name: "fk_anime_notifications_dubbing_dubbing_id",
                table: "anime_notifications",
                column: "dubbing_id",
                principalTable: "dubbing",
                principalColumn: "id");
        }
    }
}
