using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class updatedAuthLevelDatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUserHasAuthLevel_AspNetUsers_UserId",
                "ApplicationUserHasAuthLevel");

            migrationBuilder.DropIndex(
                "IX_ApplicationUserHasAuthLevel_UserId",
                "ApplicationUserHasAuthLevel");

            migrationBuilder.DropColumn(
                "UserId",
                "ApplicationUserHasAuthLevel");

            //try
            //{
            //    migrationBuilder.DropTable("PasswordResets");
            //}
            //catch
            //{
            //}

            migrationBuilder.CreateTable(
                "PasswordResets",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserNumber = table.Column<int>(nullable: false),
                    VertificationCode = table.Column<Guid>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_PasswordResets", x => x.Id); });

            migrationBuilder.AddColumn<string>(
                "UserNumber",
                "ApplicationUserHasAuthLevel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "UserNumber",
                "ApplicationUserHasAuthLevel");

            migrationBuilder.DropTable(
                "PasswordResets");

            migrationBuilder.AddColumn<string>(
                "UserId",
                "ApplicationUserHasAuthLevel",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUserHasAuthLevel_UserId",
                "ApplicationUserHasAuthLevel",
                "UserId");

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUserHasAuthLevel_AspNetUsers_UserId",
                "ApplicationUserHasAuthLevel",
                "UserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}