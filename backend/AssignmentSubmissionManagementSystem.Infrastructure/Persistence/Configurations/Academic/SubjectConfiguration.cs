using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class SubjectConfiguration
    : BaseEntityConfiguration<Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> builder)
    {
        base.Configure(builder);

        builder.ToTable("subjects");

        builder.HasKey(x => x.SubjectId)
            .HasName("subjects_pkey");

        builder.Property(x => x.SubjectId)
            .HasColumnName("subjectid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.SubjectCode)
            .HasColumnName("subjectcode")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.SubjectName)
            .HasColumnName("subjectname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

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
            .HasConstraintName("fk_subjects_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.SubjectCode
        })
        .IsUnique()
        .HasDatabaseName("uq_subjects_institution_code");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.SubjectId
        })
        .HasName("uq_subjects_institution_subject");

        builder.HasIndex(x => x.InstitutionId)
            .HasDatabaseName("ix_subjects_institutionid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_subjects_code",
                "length(trim(subjectcode)) > 0");

            table.HasCheckConstraint(
                "ck_subjects_name",
                "length(trim(subjectname)) > 0");
        });
    }
}