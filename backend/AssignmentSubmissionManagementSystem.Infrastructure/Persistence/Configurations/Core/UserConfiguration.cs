using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Core;

public sealed class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        builder.HasKey(x => x.UserId)
            .HasName("users_pkey");

        builder.Property(x => x.UserId)
            .HasColumnName("userid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid");

        builder.Property(x => x.RoleId)
            .HasColumnName("roleid")
            .IsRequired();

        builder.Property(x => x.FullName)
            .HasColumnName("fullname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName("passwordhash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phonenumber")
            .HasMaxLength(20);

        builder.Property(x => x.IsActive)
            .HasColumnName("isactive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("isdeleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Institution)
            .WithMany()
            .HasForeignKey(x => x.InstitutionId)
            .HasConstraintName("fk_users_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasConstraintName("fk_users_roles")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.UserId
        })
        .HasName("uq_users_institution_user");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_users_fullname",
                "length(trim(fullname)) > 0");

            table.HasCheckConstraint(
                "ck_users_email",
                "length(trim(email)) > 0");
        });
    }
}