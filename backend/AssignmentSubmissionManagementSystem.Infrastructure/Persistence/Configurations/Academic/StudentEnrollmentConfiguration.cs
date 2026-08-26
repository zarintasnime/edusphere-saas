using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class StudentEnrollmentConfiguration
    : BaseEntityConfiguration<StudentEnrollment>
{
    public override void Configure(
        EntityTypeBuilder<StudentEnrollment> builder)
    {
        base.Configure(builder);

        builder.ToTable("studentenrollments");

        builder.HasKey(x => x.EnrollmentId)
            .HasName("studentenrollments_pkey");

        builder.Property(x => x.EnrollmentId)
            .HasColumnName("enrollmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.StudentId)
            .HasColumnName("studentid")
            .IsRequired();

        builder.Property(x => x.AcademicYearId)
            .HasColumnName("academicyearid")
            .IsRequired();

        builder.Property(x => x.RollNumber)
            .HasColumnName("rollnumber")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EnrollmentDate)
            .HasColumnName("enrollmentdate")
            .HasColumnType("date");

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.StudentId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.StudentId
            })
            .HasConstraintName(
                "fk_studentenrollments_institutionstudents")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcademicYear)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.AcademicYearId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.AcademicYearId
            })
            .HasConstraintName(
                "fk_studentenrollments_institutionacademicyears")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.StudentId,
            x.AcademicYearId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_studentenrollments_institution_student_academicyear");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.AcademicYearId,
            x.RollNumber
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_studentenrollments_institution_academicyear_roll");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.EnrollmentId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_studentenrollments_institution_enrollment");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.StudentId
        })
        .IsUnique()
        .HasFilter("isactive = TRUE")
        .HasDatabaseName(
            "uq_studentenrollments_onlyoneactive");

        builder.HasIndex(x => x.AcademicYearId)
            .HasDatabaseName(
                "ix_studentenrollments_academicyearid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_studentenrollments_roll",
                "length(trim(rollnumber)) > 0");
        });
    }
}