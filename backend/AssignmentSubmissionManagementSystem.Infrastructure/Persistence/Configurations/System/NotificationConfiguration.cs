using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.System;

public sealed class NotificationConfiguration
    : BaseEntityConfiguration<Notification>
{
    public override void Configure(
        EntityTypeBuilder<Notification> builder)
    {
        base.Configure(builder);

        builder.ToTable("notifications");

        builder.HasKey(x => x.NotificationId)
            .HasName("notifications_pkey");

        builder.Property(x => x.NotificationId)
            .HasColumnName("notificationid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid");

        builder.Property(x => x.UserId)
            .HasColumnName("userid")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasColumnName("message")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.NotificationType)
            .HasColumnName("notificationtype")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Channel)
            .HasConversion<string>()
            .HasColumnName("channel")
            .HasMaxLength(20)
            .HasDefaultValue(NotificationChannel.InApp)
            .IsRequired();

        builder.Property(x => x.ReferenceId)
            .HasColumnName("referenceid");

        builder.Property(x => x.IsRead)
            .HasColumnName("isread")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.ReadAt)
            .HasColumnName("readat")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(x => x.Institution)
            .WithMany()
            .HasForeignKey(x => x.InstitutionId)
            .HasConstraintName("fk_notifications_institutions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_notifications_users")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.UserId,
            x.IsRead
        })
        .HasDatabaseName(
            "ix_notifications_institution_user_isread");

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_notifications_title",
                "length(trim(title)) > 0");

            table.HasCheckConstraint(
                "ck_notifications_message",
                "length(trim(message)) > 0");

            table.HasCheckConstraint(
                "ck_notifications_channel",
                "channel IN ('InApp', 'Email', 'SMS', 'Push')");

            table.HasCheckConstraint(
                "ck_notifications_readstate",
                """
                (isread = FALSE AND readat IS NULL)
                OR
                (isread = TRUE AND readat IS NOT NULL)
                """);
        });
    }
}