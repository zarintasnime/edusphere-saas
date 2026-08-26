using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class AssessmentConfiguration
    : BaseEntityConfiguration<Assessment>
{
    public override void Configure(EntityTypeBuilder<Assessment> builder)
    {
        base.Configure(builder);

        builder.ToTable("assessments");

        builder.HasKey(x => x.AssessmentId)
            .HasName("assessments_pkey");

        builder.Property(x => x.AssessmentId)
            .HasColumnName("assessmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.SubmissionId)
            .HasColumnName("submissionid")
            .IsRequired();

        builder.Property(x => x.TeacherId)
            .HasColumnName("teacherid")
            .IsRequired();

        builder.Property(x => x.PolicyId)
            .HasColumnName("policyid");

        builder.Property(x => x.MarksObtained)
            .HasColumnName("marksobtained")
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(x => x.PenaltyPercentageApplied)
            .HasColumnName("penaltypercentageapplied")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.FinalMarks)
            .HasColumnName("finalmarks")
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(x => x.Feedback)
            .HasColumnName("feedback")
            .HasColumnType("text");

        builder.Property(x => x.ReviewedAt)
            .HasColumnName("reviewedat")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Submission)
            .WithOne()
            .HasPrincipalKey<Submission>(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasForeignKey<Assessment>(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasConstraintName(
                "fk_assessments_institutionsubmissions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Teacher)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.TeacherId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.TeacherId
            })
            .HasConstraintName(
                "fk_assessments_institutionteachers")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Policy)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.PolicyId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.PolicyId
            })
            .HasConstraintName(
                "fk_assessments_institutionpolicies")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.SubmissionId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_assessments_institution_submission");

        builder.HasIndex(x => x.TeacherId)
            .HasDatabaseName("ix_assessments_teacherid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_assessments_marks",
                """
                marksobtained >= 0
                AND finalmarks >= 0
                AND finalmarks <= marksobtained
                """);

            table.HasCheckConstraint(
                "ck_assessments_penaltypercentage",
                "penaltypercentageapplied BETWEEN 0 AND 100");

            table.HasCheckConstraint(
                "ck_assessments_policyusage",
                """
                (policyid IS NULL AND penaltypercentageapplied = 0)
                OR
                (policyid IS NOT NULL)
                """);
        });
    }
}