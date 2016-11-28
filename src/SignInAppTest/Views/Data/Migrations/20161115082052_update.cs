using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ApplicationUserHasAuthLevels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHasAuthLevels_UserId",
                table: "ApplicationUserHasAuthLevels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserHasAuthLevels_UserId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApplicationUserHasAuthLevels");
        }
    }
}
