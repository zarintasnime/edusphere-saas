using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionAttachmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_submissionattachments_institutionsubmissions",
                table: "submissionattachments");

            migrationBuilder.DropIndex(
                name: "IX_submissionattachments_institutionid_submissionid",
                table: "submissionattachments");

            migrationBuilder.CreateIndex(
                name: "IX_submissionattachments_submissionid",
                table: "submissionattachments",
                column: "submissionid");

            migrationBuilder.AddForeignKey(
                name: "fk_submissionattachments_submissions",
                table: "submissionattachments",
                column: "submissionid",
                principalTable: "submissions",
                principalColumn: "submissionid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_submissionattachments_submissions",
                table: "submissionattachments");

            migrationBuilder.DropIndex(
                name: "IX_submissionattachments_submissionid",
                table: "submissionattachments");

            migrationBuilder.CreateIndex(
                name: "IX_submissionattachments_institutionid_submissionid",
                table: "submissionattachments",
                columns: new[] { "institutionid", "submissionid" });

            migrationBuilder.AddForeignKey(
                name: "fk_submissionattachments_institutionsubmissions",
                table: "submissionattachments",
                columns: new[] { "institutionid", "submissionid" },
                principalTable: "submissions",
                principalColumns: new[] { "institutionid", "submissionid" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
