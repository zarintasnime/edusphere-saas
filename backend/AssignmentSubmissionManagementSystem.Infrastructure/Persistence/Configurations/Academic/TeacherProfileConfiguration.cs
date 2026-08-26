using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class TeacherProfileConfiguration : BaseEntityConfiguration<TeacherProfile>
{
    public override void Configure(EntityTypeBuilder<TeacherProfile> builder)
    {
        base.Configure(builder);

        builder.ToTable("teacherprofiles");

        builder.HasKey(x => x.TeacherId)
            .HasName("teacherprofiles_pkey");

        builder.Property(x => x.TeacherId)
            .HasColumnName("teacherid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("userid")
            .IsRequired();

        builder.Property(x => x.DepartmentId)
            .HasColumnName("departmentid")
            .IsRequired();

        builder.Property(x => x.EmployeeCode)
            .HasColumnName("employeecode")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Qualification)
            .HasColumnName("qualification")
            .HasMaxLength(150);

        builder.Property(x => x.JoiningDate)
            .HasColumnName("joiningdate");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.UserId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.UserId
            })
            .HasConstraintName("fk_teacherprofiles_institutionusers")
            .OnDelete(DeleteBehavior.Restrict);

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
            .HasConstraintName("fk_teacherprofiles_institutiondepartments")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.UserId
        })
        .IsUnique()
        .HasDatabaseName("uq_teacherprofiles_institution_user");

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.EmployeeCode
        })
        .IsUnique()
        .HasDatabaseName("uq_teacherprofiles_institution_employeecode");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.TeacherId
        })
        .HasName("uq_teacherprofiles_institution_teacher");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_teacherprofiles_employeecode",
                "length(trim(employeecode)) > 0");
        });
    }
}