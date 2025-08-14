using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class createLinkToMasterField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_like_inventory_object_fk_inventory_id",
                table: "like");

            migrationBuilder.RenameColumn(
                name: "fk_inventory_id",
                table: "like",
                newName: "fk_inventory_object_id");

            migrationBuilder.RenameIndex(
                name: "IX_like_fk_inventory_id",
                table: "like",
                newName: "IX_like_fk_inventory_object_id");

            migrationBuilder.AddColumn<Guid>(
                name: "fk_master_object_field_id",
                table: "object_field",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_object_field_fk_master_object_field_id",
                table: "object_field",
                column: "fk_master_object_field_id");

            migrationBuilder.AddForeignKey(
                name: "FK_like_inventory_object_fk_inventory_object_id",
                table: "like",
                column: "fk_inventory_object_id",
                principalTable: "inventory_object",
                principalColumn: "pk_inventory_object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_object_field_object_field_fk_master_object_field_id",
                table: "object_field",
                column: "fk_master_object_field_id",
                principalTable: "object_field",
                principalColumn: "pk_object_field_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_like_inventory_object_fk_inventory_object_id",
                table: "like");

            migrationBuilder.DropForeignKey(
                name: "FK_object_field_object_field_fk_master_object_field_id",
                table: "object_field");

            migrationBuilder.DropIndex(
                name: "IX_object_field_fk_master_object_field_id",
                table: "object_field");

            migrationBuilder.DropColumn(
                name: "fk_master_object_field_id",
                table: "object_field");

            migrationBuilder.RenameColumn(
                name: "fk_inventory_object_id",
                table: "like",
                newName: "fk_inventory_id");

            migrationBuilder.RenameIndex(
                name: "IX_like_fk_inventory_object_id",
                table: "like",
                newName: "IX_like_fk_inventory_id");

            migrationBuilder.AddForeignKey(
                name: "FK_like_inventory_object_fk_inventory_id",
                table: "like",
                column: "fk_inventory_id",
                principalTable: "inventory_object",
                principalColumn: "pk_inventory_object_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
