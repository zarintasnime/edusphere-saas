using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class DepartmentConfiguration : BaseEntityConfiguration<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        base.Configure(builder);

        builder.ToTable("departments");

        builder.HasKey(x => x.DepartmentId)
            .HasName("departments_pkey");

        builder.Property(x => x.DepartmentId)
            .HasColumnName("departmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.DepartmentCode)
            .HasColumnName("departmentcode")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.DepartmentName)
            .HasColumnName("departmentname")
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
            .HasConstraintName("fk_departments_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.DepartmentCode
        })
        .IsUnique()
        .HasDatabaseName("uq_departments_institution_code");

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.DepartmentId
        })
        .HasName("uq_departments_institution_department");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_departments_code",
                "length(trim(departmentcode)) > 0");

            table.HasCheckConstraint(
                "ck_departments_name",
                "length(trim(departmentname)) > 0");
        });
    }
}