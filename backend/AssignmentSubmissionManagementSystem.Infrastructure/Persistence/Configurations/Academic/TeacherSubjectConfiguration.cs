using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class TeacherSubjectConfiguration
    : BaseEntityConfiguration<TeacherSubject>
{
    public override void Configure(
        EntityTypeBuilder<TeacherSubject> builder)
    {
        base.Configure(builder);

        builder.ToTable("teachersubjects");

        builder.HasKey(x => x.TeacherSubjectId)
            .HasName("teachersubjects_pkey");

        builder.Property(x => x.TeacherSubjectId)
            .HasColumnName("teachersubjectid")
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
                "fk_teachersubjects_institutionteachers")
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
                "fk_teachersubjects_institutioncoursesubjects")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.TeacherId,
            x.CourseSubjectId
        })
        .HasName(
            "uq_teachersubjects_institution_teacher_coursesubject");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.TeacherSubjectId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_teachersubjects_institution_teachersubject");

        builder.HasIndex(x => x.CourseSubjectId)
            .HasDatabaseName(
                "ix_teachersubjects_coursesubjectid");
    }
}