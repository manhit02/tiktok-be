using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TikTok.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CheckVideoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoLikes_Users_UserId",
                table: "VideoLikes");

            migrationBuilder.DropColumn(
                name: "NguoiTheoDoi",
                table: "Users");

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

            migrationBuilder.AddForeignKey(
                name: "FK_VideoLikes_Users_UserId",
                table: "VideoLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Videos_VideoId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoLikes_Users_UserId",
                table: "VideoLikes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_VideoId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "VideoId1",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "NguoiTheoDoi",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoLikes_Users_UserId",
                table: "VideoLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
