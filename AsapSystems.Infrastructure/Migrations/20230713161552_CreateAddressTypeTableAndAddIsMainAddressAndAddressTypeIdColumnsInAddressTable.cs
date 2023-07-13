using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsapSystems.Infrastructure.Migrations
{
    public partial class CreateAddressTypeTableAndAddIsMainAddressAndAddressTypeIdColumnsInAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressTypeId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMainAddress",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AddressTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AddressTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Home" });

            migrationBuilder.InsertData(
                table: "AddressTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Work" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressTypeId",
                table: "Addresses",
                column: "AddressTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressTypes_AddressTypeId",
                table: "Addresses",
                column: "AddressTypeId",
                principalTable: "AddressTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressTypes_AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "AddressTypes");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsMainAddress",
                table: "Addresses");
        }
    }
}
