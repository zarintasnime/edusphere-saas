using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.System;

public sealed class AuditLogConfiguration
    : BaseEntityConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.ToTable("auditlogs");

        builder.HasKey(x => x.AuditLogId)
            .HasName("auditlogs_pkey");

        builder.Property(x => x.AuditLogId)
            .HasColumnName("auditlogid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid");

        builder.Property(x => x.UserId)
            .HasColumnName("userid")
            .IsRequired();

        builder.Property(x => x.Action)
            .HasConversion<string>()
            .HasColumnName("action")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EntityName)
            .HasColumnName("entityname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.EntityId)
            .HasColumnName("entityid")
            .IsRequired();

        builder.Property(x => x.OldValues)
            .HasColumnName("oldvalues")
            .HasColumnType("jsonb");

        builder.Property(x => x.NewValues)
            .HasColumnName("newvalues")
            .HasColumnType("jsonb");

        builder.HasOne(x => x.Institution)
            .WithMany()
            .HasForeignKey(x => x.InstitutionId)
            .HasConstraintName("fk_auditlogs_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_auditlogs_users")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.UserId,
            x.CreatedAt
        })
        .HasDatabaseName(
            "ix_auditlogs_institution_user_createdat");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_auditlogs_action",
                """
                action IN
                (
                    'CREATE',
                    'UPDATE',
                    'DELETE',
                    'APPROVE',
                    'REJECT',
                    'SUBMIT',
                    'REVIEW',
                    'LOGIN',
                    'LOGOUT',
                    'PUBLISH'
                )
                """);

            table.HasCheckConstraint(
                "ck_auditlogs_entityname",
                "length(trim(entityname)) > 0");

            table.HasCheckConstraint(
                "ck_auditlogs_entityid",
                "entityid > 0");
        });
    }
}