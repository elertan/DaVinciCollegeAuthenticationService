using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class UpdatedValidForAndAddedExpiryExtendingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExtendExpiryOnRequest",
                table: "Applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ValidFor",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtendExpiryOnRequest",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "ValidFor",
                table: "Applications",
                nullable: true);
        }
    }
}
