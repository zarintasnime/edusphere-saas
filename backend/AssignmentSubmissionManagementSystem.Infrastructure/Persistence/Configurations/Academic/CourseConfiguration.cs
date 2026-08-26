using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class CourseConfiguration
    : BaseEntityConfiguration<Course>
{
    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        base.Configure(builder);

        builder.ToTable("courses");

        builder.HasKey(x => x.CourseId)
            .HasName("courses_pkey");

        builder.Property(x => x.CourseId)
            .HasColumnName("courseid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.DepartmentId)
            .HasColumnName("departmentid")
            .IsRequired();

        builder.Property(x => x.CourseCode)
            .HasColumnName("coursecode")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CourseName)
            .HasColumnName("coursename")
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

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.DepartmentId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.DepartmentId
            })
            .HasConstraintName(
                "fk_courses_institutiondepartments")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.CourseCode
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_courses_institution_code");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.CourseId
        })
        .HasName(
            "uq_courses_institution_course");

        builder.HasIndex(x => x.DepartmentId)
            .HasDatabaseName("ix_courses_departmentid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_courses_code",
                "length(trim(coursecode)) > 0");

            table.HasCheckConstraint(
                "ck_courses_name",
                "length(trim(coursename)) > 0");
        });
    }
}