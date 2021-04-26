using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class ForgetAboutId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TaskForEmployee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskForEmployee");
        }
    }
}
