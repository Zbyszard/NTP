using Microsoft.EntityFrameworkCore.Migrations;

namespace Kopyw.Infrastructure.DataAccess.Migrations
{
    public partial class AddImageRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MessageImageId",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PostImageId",
                table: "Images",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(nullable: false),
                    ImageId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageImages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(nullable: false),
                    ImageId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_MessageImageId",
                table: "Images",
                column: "MessageImageId",
                unique: true,
                filter: "[MessageImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostImageId",
                table: "Images",
                column: "PostImageId",
                unique: true,
                filter: "[PostImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MessageImages_MessageId",
                table: "MessageImages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_PostId",
                table: "PostImages",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_MessageImages_MessageImageId",
                table: "Images",
                column: "MessageImageId",
                principalTable: "MessageImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_PostImages_PostImageId",
                table: "Images",
                column: "PostImageId",
                principalTable: "PostImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_MessageImages_MessageImageId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_PostImages_PostImageId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "MessageImages");

            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropIndex(
                name: "IX_Images_MessageImageId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_PostImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MessageImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PostImageId",
                table: "Images");
        }
    }
}
