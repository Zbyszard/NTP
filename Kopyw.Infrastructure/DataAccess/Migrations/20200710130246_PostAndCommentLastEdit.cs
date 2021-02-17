using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kopyw.Infrastructure.DataAccess.Migrations
{
    public partial class PostAndCommentLastEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditTime",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditTime",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditTime",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LastEditTime",
                table: "Comments");
        }
    }
}
