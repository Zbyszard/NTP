using Microsoft.EntityFrameworkCore.Migrations;

namespace Kopyw.Infrastructure.DataAccess.Migrations
{
    public partial class CommentVoteValueToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "CommentVotes",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Value",
                table: "CommentVotes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
