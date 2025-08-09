using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_account_fk_creator_id",
                table: "comment");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_account_TempId",
                table: "account");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "account");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_account_fk_creator_id",
                table: "comment",
                column: "fk_creator_id",
                principalTable: "account",
                principalColumn: "pk_account_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_account_fk_creator_id",
                table: "comment");

            migrationBuilder.AddColumn<string>(
                name: "TempId",
                table: "account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_account_TempId",
                table: "account",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_account_fk_creator_id",
                table: "comment",
                column: "fk_creator_id",
                principalTable: "account",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
