using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTasks_AspNetUsers_ApplicationUserId",
                table: "EmployeeTasks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTasks_ApplicationUserId",
                table: "EmployeeTasks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "EmployeeTasks");

            migrationBuilder.CreateTable(
                name: "ApplicationUserEmployeeTask",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEmployeeTask", x => new { x.EmployeeId, x.EmployeeTaskId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEmployeeTask_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEmployeeTask_EmployeeTasks_EmployeeTaskId",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEmployeeTask_EmployeeTaskId",
                table: "ApplicationUserEmployeeTask",
                column: "EmployeeTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEmployeeTask");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "EmployeeTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTasks_ApplicationUserId",
                table: "EmployeeTasks",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTasks_AspNetUsers_ApplicationUserId",
                table: "EmployeeTasks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
