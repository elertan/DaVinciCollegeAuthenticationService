using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class changedPasswordResetColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNumber",
                table: "PasswordResets");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PasswordResets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PasswordResets");

            migrationBuilder.AddColumn<int>(
                name: "UserNumber",
                table: "PasswordResets",
                nullable: false,
                defaultValue: 0);
        }
    }
}
