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
                    ISOCode = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.ISOCode);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Login = table.Column<string>(maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false),
                    Location_CountryISOCode = table.Column<string>(maxLength: 2, nullable: false),
                    Location_ProvinceISOCode = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ISOCode = table.Column<string>(maxLength: 3, nullable: false),
                    CountryISOCode = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => new { x.CountryISOCode, x.ISOCode });
                    table.ForeignKey(
                        name: "FK_Province_Country_CountryISOCode",
                        column: x => x.CountryISOCode,
                        principalTable: "Country",
                        principalColumn: "ISOCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "ISOCode", "Name" },
                values: new object[,]
                {
                    { "RU", "Russian Federation" },
                    { "US", "United States of America" },
                    { "CN", "People's Republic of China" }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "CountryISOCode", "ISOCode", "Name" },
                values: new object[,]
                {
                    { "RU", "MOS", "Moskovskaya oblast'" },
                    { "RU", "LEN", "Leningradskaya oblast'" },
                    { "RU", "NVS", "Novosibirskaya oblast'" },
                    { "US", "NY", "New York" },
                    { "US", "WA", "Washington" },
                    { "US", "CA", "California" },
                    { "CN", "HK", "Hong Kong" },
                    { "CN", "HL", "Harbin" },
                    { "CN", "JS", "Nanjing" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
