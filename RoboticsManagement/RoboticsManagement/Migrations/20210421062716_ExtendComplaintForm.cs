using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoboticsManagement.Migrations
{
    public partial class ExtendComplaintForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "complaintFormModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "complaintFormModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "complaintFormModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "complaintFormModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "complaintFormModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "complaintFormModels");

            migrationBuilder.DropColumn(
                name: "City",
                table: "complaintFormModels");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "complaintFormModels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "complaintFormModels");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "complaintFormModels");
        }
    }
}
