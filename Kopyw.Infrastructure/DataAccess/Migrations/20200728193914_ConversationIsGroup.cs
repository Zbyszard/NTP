using Microsoft.EntityFrameworkCore.Migrations;

namespace Kopyw.Infrastructure.DataAccess.Migrations
{
    public partial class ConversationIsGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "Conversations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "Conversations");
        }
    }
}
