using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class AcademicYearConfiguration
    : BaseEntityConfiguration<AcademicYear>
{
    public override void Configure(EntityTypeBuilder<AcademicYear> builder)
    {
        base.Configure(builder);

        builder.ToTable("academicyears");

        builder.HasKey(x => x.AcademicYearId)
            .HasName("academicyears_pkey");

        builder.Property(x => x.AcademicYearId)
            .HasColumnName("academicyearid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.BatchId)
            .HasColumnName("batchid")
            .IsRequired();

        builder.Property(x => x.YearName)
            .HasColumnName("yearname")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.YearOrder)
            .HasColumnName("yearorder")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Batch)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.BatchId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.BatchId
            })
            .HasConstraintName(
                "fk_academicyears_institutionbatches")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.BatchId,
            x.YearName
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_academicyears_institution_batch_yearname");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.BatchId,
            x.YearOrder
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_academicyears_institution_batch_yearorder");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.AcademicYearId
        })
        .HasName(
            "uq_academicyears_institution_academicyear");

        builder.HasIndex(x => x.BatchId)
            .HasDatabaseName("ix_academicyears_batchid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_academicyears_name",
                "length(trim(yearname)) > 0");

            table.HasCheckConstraint(
                "ck_academicyears_yearorder",
                "yearorder > 0");
        });
    }
}