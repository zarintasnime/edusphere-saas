using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherSubjectIdToAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TeacherSubjectId",
                table: "assignments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherSubjectId",
                table: "assignments");
        }
    }
}
