using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Core;

public sealed class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.ToTable("roles");

        builder.HasKey(x => x.RoleId)
            .HasName("roles_pkey");

        builder.Property(x => x.RoleId)
            .HasColumnName("roleid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RoleName)
            .HasConversion<string>()
            .HasColumnName("rolename")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.RoleName)
            .IsUnique()
            .HasDatabaseName("roles_rolename_key");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_roles_name",
                "length(trim(rolename)) > 0");
        });
    }
}