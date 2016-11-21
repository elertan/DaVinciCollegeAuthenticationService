using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class addeddbsetforauthlevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevel_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserHasAuthLevel",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserHasAuthLevels",
                table: "ApplicationUserHasAuthLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevel",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserHasAuthLevel_ApplicationId",
                table: "ApplicationUserHasAuthLevel",
                newName: "IX_ApplicationUserHasAuthLevels_ApplicationId");

            migrationBuilder.RenameTable(
                name: "ApplicationUserHasAuthLevel",
                newName: "ApplicationUserHasAuthLevels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevels_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserHasAuthLevels",
                table: "ApplicationUserHasAuthLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserHasAuthLevel",
                table: "ApplicationUserHasAuthLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevel_Applications_ApplicationId",
                table: "ApplicationUserHasAuthLevels",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserHasAuthLevels_ApplicationId",
                table: "ApplicationUserHasAuthLevels",
                newName: "IX_ApplicationUserHasAuthLevel_ApplicationId");

            migrationBuilder.RenameTable(
                name: "ApplicationUserHasAuthLevels",
                newName: "ApplicationUserHasAuthLevel");
        }
    }
}
