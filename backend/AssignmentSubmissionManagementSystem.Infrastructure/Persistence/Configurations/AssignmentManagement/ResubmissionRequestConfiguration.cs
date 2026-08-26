using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class ResubmissionRequestConfiguration
    : BaseEntityConfiguration<ResubmissionRequest>
{
    public override void Configure(
        EntityTypeBuilder<ResubmissionRequest> builder)
    {
        base.Configure(builder);

        builder.ToTable("resubmissionrequests");

        builder.HasKey(x => x.RequestId)
            .HasName("resubmissionrequests_pkey");

        builder.Property(x => x.RequestId)
            .HasColumnName("requestid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.SubmissionId)
            .HasColumnName("submissionid")
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Submission)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasConstraintName(
                "fk_resubmissionrequests_institutionsubmissions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.RequestId
        })
        .HasName(
            "uq_resubmissionrequests_institution_request");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_resubmissionrequests_reason",
                "length(trim(reason)) > 0");
        });
    }
}