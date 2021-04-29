using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class addO2Mrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "complaintFormModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_complaintFormModels_ApplicationUserId",
                table: "complaintFormModels",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_complaintFormModels_AspNetUsers_ApplicationUserId",
                table: "complaintFormModels",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_complaintFormModels_AspNetUsers_ApplicationUserId",
                table: "complaintFormModels");

            migrationBuilder.DropIndex(
                name: "IX_complaintFormModels_ApplicationUserId",
                table: "complaintFormModels");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "complaintFormModels");
        }
    }
}
