using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class AssignmentConfiguration
    : BaseEntityConfiguration<Assignment>
{
    public override void Configure(EntityTypeBuilder<Assignment> builder)
    {
        base.Configure(builder);

        builder.ToTable("assignments");

        builder.HasKey(x => x.AssignmentId)
            .HasName("assignments_pkey");

        builder.Property(x => x.AssignmentId)
            .HasColumnName("assignmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.TeacherId)
            .HasColumnName("teacherid")
            .IsRequired();

        builder.Property(x => x.CourseSubjectId)
            .HasColumnName("coursesubjectid")
            .IsRequired();

        builder.Property(x => x.AcademicYearId)
            .HasColumnName("academicyearid")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(x => x.TotalMarks)
            .HasColumnName("totalmarks")
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .HasColumnName("duedate")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(x => x.AllowLateSubmission)
            .HasColumnName("allowlatesubmission")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.LateSubmissionDeadline)
            .HasColumnName("latesubmissiondeadline")
            .HasColumnType("timestamp without time zone");

        builder.Property(x => x.AssignmentStatus)
            .HasConversion<string>()
            .HasColumnName("assignmentstatus")
            .HasMaxLength(20)
            .HasDefaultValue(AssignmentStatus.Draft)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

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
                "fk_assignments_institutionteachers")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CourseSubject)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.CourseSubjectId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.CourseSubjectId
            })
            .HasConstraintName(
                "fk_assignments_institutioncoursesubjects")
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
                "fk_assignments_institutionacademicyears")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TeacherSubject)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.TeacherId,
                x.CourseSubjectId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.TeacherId,
                x.CourseSubjectId
            })
            .HasConstraintName(
                "fk_assignments_institutionteachersubjects")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.AssignmentId
        })
        .HasName(
            "uq_assignments_institution_assignment");

        builder.HasIndex(x => x.AcademicYearId)
            .HasDatabaseName(
                "ix_assignments_academicyearid");

        builder.HasIndex(x => x.CourseSubjectId)
            .HasDatabaseName(
                "ix_assignments_coursesubjectid");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_assignments_title",
                "length(trim(title)) > 0");

            table.HasCheckConstraint(
                "ck_assignments_totalmarks",
                "totalmarks > 0");

            table.HasCheckConstraint(
                "ck_assignments_status",
                "assignmentstatus IN ('Draft', 'Published', 'Closed', 'Archived')");

            table.HasCheckConstraint(
                "ck_assignments_latesubmission",
                """
                (allowlatesubmission = FALSE AND latesubmissiondeadline IS NULL)
                OR
                (allowlatesubmission = TRUE
                 AND latesubmissiondeadline IS NOT NULL
                 AND latesubmissiondeadline > duedate)
                """);
        });
    }
}