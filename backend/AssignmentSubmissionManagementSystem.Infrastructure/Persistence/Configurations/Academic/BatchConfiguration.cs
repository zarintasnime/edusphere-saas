using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class BatchConfiguration
    : BaseEntityConfiguration<Batch>
{
    public override void Configure(EntityTypeBuilder<Batch> builder)
    {
        base.Configure(builder);

        builder.ToTable("batches");

        builder.HasKey(x => x.BatchId)
            .HasName("batches_pkey");

        builder.Property(x => x.BatchId)
            .HasColumnName("batchid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.CourseId)
            .HasColumnName("courseid")
            .IsRequired();

        builder.Property(x => x.BatchCode)
            .HasColumnName("batchcode")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.BatchName)
            .HasColumnName("batchname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.StartYear)
            .HasColumnName("startyear")
            .IsRequired();

        builder.Property(x => x.EndYear)
            .HasColumnName("endyear");

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.CourseId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.CourseId
            })
            .HasConstraintName("fk_batches_institutioncourses")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.BatchCode
        })
        .IsUnique()
        .HasDatabaseName("uq_batches_institution_code");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.BatchId
        })
        .HasName("uq_batches_institution_batch");

        builder.HasIndex(x => x.CourseId)
            .HasDatabaseName("ix_batches_courseid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_batches_code",
                "length(trim(batchcode)) > 0");

            table.HasCheckConstraint(
                "ck_batches_name",
                "length(trim(batchname)) > 0");

            table.HasCheckConstraint(
                "ck_batches_yearrange",
                "endyear IS NULL OR endyear >= startyear");
        });
    }
}