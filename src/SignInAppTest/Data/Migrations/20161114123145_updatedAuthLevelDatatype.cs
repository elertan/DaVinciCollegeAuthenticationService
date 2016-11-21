using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class updatedAuthLevelDatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserHasAuthLevel_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserHasAuthLevel_UserId",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserNumber = table.Column<int>(nullable: false),
                    VertificationCode = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "UserNumber",
                table: "ApplicationUserHasAuthLevel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNumber",
                table: "ApplicationUserHasAuthLevel");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ApplicationUserHasAuthLevel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHasAuthLevel_UserId",
                table: "ApplicationUserHasAuthLevel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserHasAuthLevel_AspNetUsers_UserId",
                table: "ApplicationUserHasAuthLevel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
