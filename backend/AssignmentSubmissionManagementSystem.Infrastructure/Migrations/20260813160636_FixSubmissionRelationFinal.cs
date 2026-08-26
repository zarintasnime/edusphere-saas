using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSubmissionRelationFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "fk_submissions_institutionstudents",
                table: "submissions");


            migrationBuilder.AddForeignKey(
                name: "fk_submissions_assignments",
                table: "submissions",
                column: "assignmentid",
                principalTable: "assignments",
                principalColumn: "assignmentid",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.AddForeignKey(
                name: "fk_submissions_students",
                table: "submissions",
                column: "studentid",
                principalTable: "studentprofiles",
                principalColumn: "studentid",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "fk_submissions_assignments",
                table: "submissions");


            migrationBuilder.DropForeignKey(
                name: "fk_submissions_students",
                table: "submissions");



            migrationBuilder.AddForeignKey(
                name: "fk_submissions_institutionstudents",
                table: "submissions",
                columns: new[]
                {
            "institutionid",
            "studentid"
                },
                principalTable: "studentprofiles",
                principalColumns: new[]
                {
            "institutionid",
            "studentid"
                },
                onDelete: ReferentialAction.Restrict);

        }
    }
}
