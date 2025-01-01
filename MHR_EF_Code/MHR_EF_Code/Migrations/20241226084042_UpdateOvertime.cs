using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MHR_EF_Code.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOvertime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OvertimeId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Overtime",
                columns: table => new
                {
                    OvertimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OvertimeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Overtime", x => x.OvertimeId);
                    table.ForeignKey(
                        name: "FK_Overtime_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Overtime_EmployeeId",
                table: "Overtime",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Overtime");

            migrationBuilder.DropColumn(
                name: "OvertimeId",
                table: "Employees");
        }
    }
}
