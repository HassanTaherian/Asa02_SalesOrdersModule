using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class TransferIsReuturnToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "InvoiceItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "Invoices");

            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "InvoiceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
