using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsapSystems.Infrastructure.Migrations
{
    public partial class AddAddressNameInAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Addresses");
        }
    }
}
