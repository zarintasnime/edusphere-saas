using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentProfileStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "studentprofiles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "fk_studentprofiles_institutions",
                table: "studentprofiles",
                column: "institutionid",
                principalTable: "institutions",
                principalColumn: "institutionid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_studentprofiles_institutions",
                table: "studentprofiles");

            migrationBuilder.DropColumn(
                name: "isactive",
                table: "studentprofiles");
        }
    }
}
