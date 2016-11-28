using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class AddedValidFr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidFor",
                table: "Applications",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Secret",
                table: "Applications",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Applications",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidFor",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Secret",
                table: "Applications",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Applications",
                nullable: true);
        }
    }
}
