using Microsoft.EntityFrameworkCore.Migrations;

namespace Canocorean.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    ISOCode = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.ISOCode);
                });

            migrationBuilder.InsertData(
                table: "Country",
                column: "ISOCode",
                values: new object[]
                {
                    "RU",
                    "US"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
