using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceService.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDescriptionToInvoiceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "InvoiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "InvoiceItems");
        }
    }
}
