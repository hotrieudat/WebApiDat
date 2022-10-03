using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiDat.Migrations
{
    public partial class Update_refresh_token_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_users_UserId",
                table: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "IsRevokrd",
                table: "refresh_token",
                newName: "IsUsed");

            migrationBuilder.RenameColumn(
                name: "ExpriredAt",
                table: "refresh_token",
                newName: "IssuedAt");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refresh_token",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "refresh_token",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "refresh_token",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_users_UserId",
                table: "refresh_token",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_users_UserId",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "IssuedAt",
                table: "refresh_token",
                newName: "ExpriredAt");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "refresh_token",
                newName: "IsRevokrd");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refresh_token",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_users_UserId",
                table: "refresh_token",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
