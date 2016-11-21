using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserHasAuthLevels_ApplicationId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserHasAuthLevels_UserId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.AddColumn<int>(
                name: "AppId",
                table: "ApplicationUserHasAuthLevels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHasAuthLevels_AppId",
                table: "ApplicationUserHasAuthLevels",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_AppId",
                table: "ApplicationUserHasAuthLevels",
                column: "AppId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_AppId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserHasAuthLevels_AppId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "ApplicationUserHasAuthLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ApplicationUserHasAuthLevels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHasAuthLevels_ApplicationId",
                table: "ApplicationUserHasAuthLevels",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHasAuthLevels_UserId",
                table: "ApplicationUserHasAuthLevels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevels",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
