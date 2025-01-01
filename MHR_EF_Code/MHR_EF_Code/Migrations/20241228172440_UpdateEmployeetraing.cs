using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MHR_EF_Code.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeetraing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTraining_EmployeeId",
                table: "EmployeeTraining",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeTraining_EmployeeId",
                table: "EmployeeTraining");
        }
    }
}
