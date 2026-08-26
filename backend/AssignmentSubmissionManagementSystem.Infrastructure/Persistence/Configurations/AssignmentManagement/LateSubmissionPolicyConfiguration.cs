using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class LateSubmissionPolicyConfiguration
    : BaseEntityConfiguration<LateSubmissionPolicy>
{
    public override void Configure(
        EntityTypeBuilder<LateSubmissionPolicy> builder)
    {
        base.Configure(builder);

        builder.ToTable("latesubmissionpolicies");

        builder.HasKey(x => x.PolicyId)
            .HasName("latesubmissionpolicies_pkey");

        builder.Property(x => x.PolicyId)
            .HasColumnName("policyid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.PolicyName)
            .HasColumnName("policyname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PenaltyPercentage)
            .HasColumnName("penaltypercentage")
            .HasDefaultValue(25)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Institution)
            .WithMany()
            .HasForeignKey(x => x.InstitutionId)
            .HasConstraintName(
                "fk_latesubmissionpolicies_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.PolicyName
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_latesubmissionpolicies_institution_name");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.PolicyId
        })
        .HasName(
            "uq_latesubmissionpolicies_institution_policy");

        builder.HasIndex(x => x.InstitutionId)
            .IsUnique()
            .HasFilter("isactive = TRUE")
            .HasDatabaseName(
                "uq_latesubmissionpolicies_onlyoneactive");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_latesubmissionpolicies_name",
                "length(trim(policyname)) > 0");

            table.HasCheckConstraint(
                "ck_latesubmissionpolicies_penalty",
                "penaltypercentage BETWEEN 0 AND 100");
        });
    }
}