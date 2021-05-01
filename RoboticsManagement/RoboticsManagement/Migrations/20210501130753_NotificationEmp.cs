using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class NotificationEmp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeNotifications",
                columns: table => new
                {
                    NotiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromRole = table.Column<int>(type: "int", nullable: false),
                    ToEmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotiBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeNotifications", x => x.NotiId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeNotifications");
        }
    }
}
