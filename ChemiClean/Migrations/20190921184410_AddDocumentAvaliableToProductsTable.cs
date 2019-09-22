using Microsoft.EntityFrameworkCore.Migrations;

namespace ChemiClean.Migrations
{
    public partial class AddDocumentAvaliableToProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocumentAvaliable",
                table: "tblProduct",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentAvaliable",
                table: "tblProduct");
        }
    }
}
