using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class ResubmissionApprovalConfiguration
    : BaseEntityConfiguration<ResubmissionApproval>
{
    public override void Configure(
        EntityTypeBuilder<ResubmissionApproval> builder)
    {
        base.Configure(builder);

        builder.ToTable("resubmissionapprovals");

        builder.HasKey(x => x.ApprovalId)
            .HasName("resubmissionapprovals_pkey");

        builder.Property(x => x.ApprovalId)
            .HasColumnName("approvalid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.RequestId)
            .HasColumnName("requestid")
            .IsRequired();

        builder.Property(x => x.TeacherId)
            .HasColumnName("teacherid")
            .IsRequired();

        builder.Property(x => x.ApprovalStatus)
            .HasConversion<string>()
            .HasColumnName("approvalstatus")
            .HasMaxLength(20)
            .HasDefaultValue(ApprovalStatus.Pending)
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasColumnName("remarks")
            .HasColumnType("text");

        builder.Property(x => x.ReviewedAt)
            .HasColumnName("reviewedat")
            .HasColumnType("timestamp without time zone");

        builder.Property(x => x.IsUsed)
            .HasColumnName("isused")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.UsedAt)
            .HasColumnName("usedat")
            .HasColumnType("timestamp without time zone");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Request)
            .WithOne()
            .HasPrincipalKey<ResubmissionRequest>(x => new
            {
                x.InstitutionId,
                x.RequestId
            })
            .HasForeignKey<ResubmissionApproval>(x => new
            {
                x.InstitutionId,
                x.RequestId
            })
            .HasConstraintName(
                "fk_resubmissionapprovals_institutionrequests")
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
                "fk_resubmissionapprovals_institutionteachers")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.RequestId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_resubmissionapprovals_institution_request");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_resubmissionapprovals_status",
                "approvalstatus IN ('Pending', 'Approved', 'Rejected')");

            table.HasCheckConstraint(
                "ck_resubmissionapprovals_reviewedat",
                """
                (approvalstatus = 'Pending' AND reviewedat IS NULL)
                OR
                (approvalstatus IN ('Approved', 'Rejected')
                 AND reviewedat IS NOT NULL)
                """);

            table.HasCheckConstraint(
                "ck_resubmissionapprovals_usage",
                """
                (isused = FALSE AND usedat IS NULL)
                OR
                (isused = TRUE
                 AND approvalstatus = 'Approved'
                 AND usedat IS NOT NULL)
                """);
        });
    }
}