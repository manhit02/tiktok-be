using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TikTok.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Videos_VideoId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_VideoId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "VideoId1",
                table: "Comments");

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserId",
                table: "Profile",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId1",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId1",
                table: "Comments",
                column: "VideoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Videos_VideoId1",
                table: "Comments",
                column: "VideoId1",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
