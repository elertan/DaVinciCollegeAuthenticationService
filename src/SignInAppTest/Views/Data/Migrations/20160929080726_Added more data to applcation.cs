using Microsoft.EntityFrameworkCore.Migrations;

namespace DaVinciCollegeAuthenticationService.Data.Migrations
{
    public partial class Addedmoredatatoapplcation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginCallbackUrl",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCallbackUrl",
                table: "Applications");
        }
    }
}