using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Core;

public sealed class InstitutionConfiguration
    : BaseEntityConfiguration<Institution>
{
    public override void Configure(EntityTypeBuilder<Institution> builder)
    {
        base.Configure(builder);

        builder.ToTable("institutions");

        builder.HasKey(x => x.InstitutionId)
            .HasName("institutions_pkey");

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionCode)
            .HasColumnName("institutioncode")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.InstitutionName)
            .HasColumnName("institutionname")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(150);

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phonenumber")
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .HasColumnName("address")
            .HasColumnType("text");

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasIndex(x => x.InstitutionCode)
            .IsUnique()
            .HasDatabaseName("institutions_institutioncode_key");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_institutions_code",
                "length(trim(institutioncode)) > 0");

            table.HasCheckConstraint(
                "ck_institutions_name",
                "length(trim(institutionname)) > 0");
        });
    }
}