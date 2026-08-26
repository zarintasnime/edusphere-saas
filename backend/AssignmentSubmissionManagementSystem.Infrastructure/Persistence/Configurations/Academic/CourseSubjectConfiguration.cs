using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class CourseSubjectConfiguration
    : BaseEntityConfiguration<CourseSubject>
{
    public override void Configure(
        EntityTypeBuilder<CourseSubject> builder)
    {
        base.Configure(builder);

        builder.ToTable("coursesubjects");

        builder.HasKey(x => x.CourseSubjectId)
            .HasName("coursesubjects_pkey");

        builder.Property(x => x.CourseSubjectId)
            .HasColumnName("coursesubjectid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.CourseId)
            .HasColumnName("courseid")
            .IsRequired();

        builder.Property(x => x.SubjectId)
            .HasColumnName("subjectid")
            .IsRequired();

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
            .HasConstraintName(
                "fk_coursesubjects_institutioncourses")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Subject)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.SubjectId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.SubjectId
            })
            .HasConstraintName(
                "fk_coursesubjects_institutionsubjects")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.CourseId,
            x.SubjectId
        })
        .IsUnique()
        .HasDatabaseName(
            "uq_coursesubjects_institution_course_subject");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.CourseSubjectId
        })
        .HasName(
            "uq_coursesubjects_institution_coursesubject");

        builder.HasIndex(x => x.SubjectId)
            .HasDatabaseName("ix_coursesubjects_subjectid");
    }
}