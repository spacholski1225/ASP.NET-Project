using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class EmployeeTaskExtendedAboutFlagIsDone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDone",
                table: "EmployeeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDone",
                table: "EmployeeTasks");
        }
    }
}
