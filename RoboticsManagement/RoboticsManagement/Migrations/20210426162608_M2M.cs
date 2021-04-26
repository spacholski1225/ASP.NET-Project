using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class M2M : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEmployeeTask");

            migrationBuilder.CreateTable(
                name: "TaskForEmployee",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskForEmployee", x => new { x.TaskId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_TaskForEmployee_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskForEmployee_EmployeeTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmployeeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskForEmployee_EmployeeId",
                table: "TaskForEmployee",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskForEmployee");

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
    }
}
