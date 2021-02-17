using Microsoft.EntityFrameworkCore.Migrations;

namespace Kopyw.Data.Migrations
{
    public partial class AddImageInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false),
                    MiniaturePath = table.Column<string>(nullable: false),
                    Format = table.Column<string>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
