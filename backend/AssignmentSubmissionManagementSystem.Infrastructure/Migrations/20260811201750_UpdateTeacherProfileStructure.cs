using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherProfileStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "teacherprofiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_teacherprofiles_institutions_institutionid",
                table: "teacherprofiles",
                column: "institutionid",
                principalTable: "institutions",
                principalColumn: "institutionid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teacherprofiles_institutions_institutionid",
                table: "teacherprofiles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "teacherprofiles");
        }
    }
}
