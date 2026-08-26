using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class SubmissionConfiguration
    : BaseEntityConfiguration<Submission>
{
    public override void Configure(EntityTypeBuilder<Submission> builder)
    {
        base.Configure(builder);

        builder.ToTable("submissions");


        builder.HasKey(x => x.SubmissionId)
            .HasName("submissions_pkey");


        builder.Property(x => x.SubmissionId)
            .HasColumnName("submissionid")
            .ValueGeneratedOnAdd();


        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();


        builder.Property(x => x.AssignmentId)
            .HasColumnName("assignmentid")
            .IsRequired();


        builder.Property(x => x.StudentId)
            .HasColumnName("studentid")
            .IsRequired();



        builder.Property(x => x.SubmissionVersion)
            .HasColumnName("submissionversion")
            .HasDefaultValue(1)
            .IsRequired();



        builder.Property(x => x.SubmissionText)
            .HasColumnName("submissiontext")
            .HasColumnType("text");



        builder.Property(x => x.SubmittedAt)
            .HasColumnName("submittedat")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();



        builder.Property(x => x.IsLateSubmission)
            .HasColumnName("islatesubmission")
            .HasDefaultValue(false);



        builder.Property(x => x.IsLatestSubmission)
            .HasColumnName("islatestsubmission")
            .HasDefaultValue(true);



        builder.Property(x => x.SubmissionStatus)
            .HasConversion<string>()
            .HasColumnName("submissionstatus")
            .HasMaxLength(20)
            .HasDefaultValue(SubmissionStatus.Submitted);



        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat");



        // Assignment FK
        builder.HasOne(x => x.Assignment)
            .WithMany()
            .HasForeignKey(x => x.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "fk_submissions_assignments");



        // Student FK
        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "fk_submissions_students");

        builder.HasMany(x => x.SubmissionAttachments)

    .WithOne(x => x.Submission)

    .HasForeignKey(x => x.SubmissionId)

    .OnDelete(DeleteBehavior.Cascade)

    .HasConstraintName(
        "fk_submissionattachments_submissions");



        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.AssignmentId,
            x.StudentId,
            x.SubmissionVersion
        })
        .IsUnique()
        .HasDatabaseName(
        "uq_submissions_institution_assignment_student_version");



        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.SubmissionId
        })
        .HasName(
        "uq_submissions_institution_submission");



        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.AssignmentId,
            x.StudentId
        })
        .IsUnique()
        .HasFilter(
        "islatestsubmission = TRUE")
        .HasDatabaseName(
        "uq_submissions_onlyonelatest");



        builder.HasIndex(x => x.StudentId)
            .HasDatabaseName(
            "ix_submissions_studentid");


        builder.HasIndex(x => x.AssignmentId)
            .HasDatabaseName(
            "ix_submissions_assignmentid");



        builder.ToTable(table =>
        {

            table.HasCheckConstraint(
                "ck_submissions_version",
                "submissionversion > 0");


            table.HasCheckConstraint(
                "ck_submissions_content",
                "submissiontext IS NULL OR length(trim(submissiontext)) > 0");

        });

    }
}