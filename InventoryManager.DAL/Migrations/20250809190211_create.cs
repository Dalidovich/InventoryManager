using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    pk_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "character varying", nullable: false),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    password = table.Column<string>(type: "character varying", nullable: false),
                    salt = table.Column<string>(type: "character varying", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    role = table.Column<short>(type: "smallint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.pk_account_id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_category",
                columns: table => new
                {
                    pk_inventory_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_category", x => x.pk_inventory_category_id);
                });

            migrationBuilder.CreateTable(
                name: "token_data",
                columns: table => new
                {
                    pk_token_data_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token_data", x => x.pk_token_data_account_id);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    pk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: false),
                    state = table.Column<short>(type: "smallint", nullable: false),
                    img_url = table.Column<string>(type: "character varying", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.pk_inventory_id);
                    table.ForeignKey(
                        name: "FK_inventory_account_fk_creator_id",
                        column: x => x.fk_creator_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_inventory_category_fk_category_id",
                        column: x => x.fk_category_id,
                        principalTable: "inventory_category",
                        principalColumn: "pk_inventory_category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_account_to_inventory",
                columns: table => new
                {
                    pk_slave_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_master_account_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_account_to_inventory", x => new { x.pk_slave_account_id, x.pk_inventory_id });
                    table.ForeignKey(
                        name: "FK_access_account_to_inventory_account_fk_master_account_id",
                        column: x => x.fk_master_account_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_account_to_inventory_account_pk_slave_account_id",
                        column: x => x.pk_slave_account_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_account_to_inventory_inventory_pk_inventory_id",
                        column: x => x.pk_inventory_id,
                        principalTable: "inventory",
                        principalColumn: "pk_inventory_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    pk_comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.pk_comment_id);
                    table.ForeignKey(
                        name: "FK_comment_account_fk_creator_id",
                        column: x => x.fk_creator_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_inventory_fk_inventory_id",
                        column: x => x.fk_inventory_id,
                        principalTable: "inventory",
                        principalColumn: "pk_inventory_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_object",
                columns: table => new
                {
                    pk_inventory_object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying", nullable: false),
                    sequence_id = table.Column<short>(type: "smallint", nullable: false),
                    is_template = table.Column<bool>(type: "boolean", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_object", x => x.pk_inventory_object_id);
                    table.ForeignKey(
                        name: "FK_inventory_object_account_fk_creator_id",
                        column: x => x.fk_creator_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_object_inventory_fk_inventory_id",
                        column: x => x.fk_inventory_id,
                        principalTable: "inventory",
                        principalColumn: "pk_inventory_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "like",
                columns: table => new
                {
                    pk_like_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like", x => x.pk_like_id);
                    table.ForeignKey(
                        name: "FK_like_inventory_object_fk_inventory_id",
                        column: x => x.fk_inventory_id,
                        principalTable: "inventory_object",
                        principalColumn: "pk_inventory_object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "object_field",
                columns: table => new
                {
                    pk_object_field_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: false),
                    visible = table.Column<bool>(type: "boolean", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_inventory_object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_object_field", x => x.pk_object_field_id);
                    table.ForeignKey(
                        name: "FK_object_field_account_fk_creator_id",
                        column: x => x.fk_creator_id,
                        principalTable: "account",
                        principalColumn: "pk_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_object_field_inventory_object_fk_inventory_object_id",
                        column: x => x.fk_inventory_object_id,
                        principalTable: "inventory_object",
                        principalColumn: "pk_inventory_object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    pk_tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fk_inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fk_creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.pk_tag_id);
                    table.ForeignKey(
                        name: "FK_tag_inventory_object_fk_inventory_id",
                        column: x => x.fk_inventory_id,
                        principalTable: "inventory_object",
                        principalColumn: "pk_inventory_object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_account_to_inventory_fk_master_account_id",
                table: "access_account_to_inventory",
                column: "fk_master_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_account_to_inventory_pk_inventory_id",
                table: "access_account_to_inventory",
                column: "pk_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_login",
                table: "account",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_fk_creator_id",
                table: "comment",
                column: "fk_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_fk_inventory_id",
                table: "comment",
                column: "fk_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_fk_category_id",
                table: "inventory",
                column: "fk_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_fk_creator_id",
                table: "inventory",
                column: "fk_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_object_fk_creator_id",
                table: "inventory_object",
                column: "fk_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_object_fk_inventory_id",
                table: "inventory_object",
                column: "fk_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_like_fk_inventory_id",
                table: "like",
                column: "fk_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_field_fk_creator_id",
                table: "object_field",
                column: "fk_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_field_fk_inventory_object_id",
                table: "object_field",
                column: "fk_inventory_object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_fk_inventory_id",
                table: "tag",
                column: "fk_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_token_data_refresh_token",
                table: "token_data",
                column: "refresh_token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_account_to_inventory");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "like");

            migrationBuilder.DropTable(
                name: "object_field");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "token_data");

            migrationBuilder.DropTable(
                name: "inventory_object");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "inventory_category");
        }
    }
}
