using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeNotificationsBot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "anime_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "anime_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dubbing",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dubbing", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mpaa_rates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mpaa_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "studios",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_studios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    telegram_user_id = table.Column<long>(type: "bigint", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: false),
                    is_remove = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "animes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_from_anime_go = table.Column<long>(type: "bigint", nullable: false),
                    title_en = table.Column<string>(type: "text", nullable: true),
                    title_ru = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    rate = table.Column<double>(type: "double precision", nullable: true),
                    count_episode = table.Column<int>(type: "integer", nullable: true),
                    planned = table.Column<int>(type: "integer", nullable: true),
                    completed = table.Column<int>(type: "integer", nullable: true),
                    watching = table.Column<int>(type: "integer", nullable: true),
                    dropped = table.Column<int>(type: "integer", nullable: true),
                    on_hold = table.Column<int>(type: "integer", nullable: true),
                    href = table.Column<string>(type: "text", nullable: true),
                    img_href = table.Column<string>(type: "text", nullable: true),
                    next_episode = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<string>(type: "text", nullable: true),
                    id_for_comments = table.Column<long>(type: "bigint", nullable: true),
                    status_id = table.Column<long>(type: "bigint", nullable: true),
                    mpaa_rate_id = table.Column<long>(type: "bigint", nullable: true),
                    type_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_animes", x => x.id);
                    table.ForeignKey(
                        name: "fk_animes_anime_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "anime_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_animes_anime_types_type_id",
                        column: x => x.type_id,
                        principalTable: "anime_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_animes_mpaa_rates_mpaa_rate_id",
                        column: x => x.mpaa_rate_id,
                        principalTable: "mpaa_rates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "anime_comments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    author_name = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    parent_comment_id = table.Column<long>(type: "bigint", nullable: true),
                    anime_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_anime_comments_anime_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "anime_comments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_anime_comments_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime_dubbing",
                columns: table => new
                {
                    animes_id = table.Column<long>(type: "bigint", nullable: false),
                    dubbing_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_dubbing", x => new { x.animes_id, x.dubbing_id });
                    table.ForeignKey(
                        name: "fk_anime_dubbing_animes_animes_id",
                        column: x => x.animes_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_dubbing_dubbing_dubbing_id",
                        column: x => x.dubbing_id,
                        principalTable: "dubbing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "anime_genre",
                columns: table => new
                {
                    animes_id = table.Column<long>(type: "bigint", nullable: false),
                    genres_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_genre", x => new { x.animes_id, x.genres_id });
                    table.ForeignKey(
                        name: "fk_anime_genre_animes_animes_id",
                        column: x => x.animes_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_genre_genres_genres_id",
                        column: x => x.genres_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime_notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title_anime = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    href = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_notified = table.Column<bool>(type: "boolean", nullable: false),
                    anime_id = table.Column<long>(type: "bigint", nullable: false),
                    dubbing_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_anime_notifications_animes_anime_id",
                        column: x => x.anime_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_notifications_dubbing_dubbing_id",
                        column: x => x.dubbing_id,
                        principalTable: "dubbing",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "anime_studio",
                columns: table => new
                {
                    animes_id = table.Column<long>(type: "bigint", nullable: false),
                    studios_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anime_studio", x => new { x.animes_id, x.studios_id });
                    table.ForeignKey(
                        name: "fk_anime_studio_animes_animes_id",
                        column: x => x.animes_id,
                        principalTable: "animes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anime_studio_studios_studios_id",
                        column: x => x.studios_id,
                        principalTable: "studios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ix_anime_comments_anime_id",
                table: "anime_comments",
                column: "anime_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_comments_comment_id",
                table: "anime_comments",
                column: "comment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_comments_parent_comment_id",
                table: "anime_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_dubbing_dubbing_id",
                table: "anime_dubbing",
                column: "dubbing_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_dubbing1_first_episode_animes_id",
                table: "anime_dubbing1",
                column: "first_episode_animes_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_genre_genres_id",
                table: "anime_genre",
                column: "genres_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_anime_id",
                table: "anime_notifications",
                column: "anime_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_notifications_dubbing_id",
                table: "anime_notifications",
                column: "dubbing_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_statuses_title",
                table: "anime_statuses",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_studio_studios_id",
                table: "anime_studio",
                column: "studios_id");

            migrationBuilder.CreateIndex(
                name: "ix_anime_types_title",
                table: "anime_types",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_anime_user_users_id",
                table: "anime_user",
                column: "users_id");

            migrationBuilder.CreateIndex(
                name: "ix_animes_id_from_anime_go",
                table: "animes",
                column: "id_from_anime_go",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_animes_mpaa_rate_id",
                table: "animes",
                column: "mpaa_rate_id");

            migrationBuilder.CreateIndex(
                name: "ix_animes_status_id",
                table: "animes",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_animes_type_id",
                table: "animes",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_dubbing_title",
                table: "dubbing",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_genres_title",
                table: "genres",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mpaa_rates_title",
                table: "mpaa_rates",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_studios_title",
                table: "studios",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_telegram_chat_id",
                table: "users",
                column: "telegram_chat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_telegram_user_id",
                table: "users",
                column: "telegram_user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anime_comments");

            migrationBuilder.DropTable(
                name: "anime_dubbing");

            migrationBuilder.DropTable(
                name: "anime_dubbing1");

            migrationBuilder.DropTable(
                name: "anime_genre");

            migrationBuilder.DropTable(
                name: "anime_notifications");

            migrationBuilder.DropTable(
                name: "anime_studio");

            migrationBuilder.DropTable(
                name: "anime_user");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "dubbing");

            migrationBuilder.DropTable(
                name: "studios");

            migrationBuilder.DropTable(
                name: "animes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "anime_statuses");

            migrationBuilder.DropTable(
                name: "anime_types");

            migrationBuilder.DropTable(
                name: "mpaa_rates");
        }
    }
}
