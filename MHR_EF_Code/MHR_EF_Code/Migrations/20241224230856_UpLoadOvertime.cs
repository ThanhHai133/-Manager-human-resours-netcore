using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MHR_EF_Code.Migrations
{
    /// <inheritdoc />
    public partial class UpLoadOvertime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance");

            migrationBuilder.CreateTable(
                name: "overtimes",
                columns: table => new
                {
                    EmployeeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OvertimeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_overtimes", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_overtimes_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "overtimes");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance",
                column: "EmployeeId");
        }
    }
}
