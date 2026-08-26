using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class SubmissionAttachmentConfiguration
    : BaseEntityConfiguration<SubmissionAttachment>
{
    public override void Configure(
        EntityTypeBuilder<SubmissionAttachment> builder)
    {
        base.Configure(builder);

        builder.ToTable("submissionattachments");

        builder.HasKey(x => x.AttachmentId)
            .HasName("submissionattachments_pkey");

        builder.Property(x => x.AttachmentId)
            .HasColumnName("attachmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.SubmissionId)
            .HasColumnName("submissionid")
            .IsRequired();

        builder.Property(x => x.FileName)
            .HasColumnName("filename")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.FilePath)
            .HasColumnName("filepath")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.FileType)
            .HasColumnName("filetype")
            .HasMaxLength(100);

        builder.Property(x => x.FileSize)
            .HasColumnName("filesize");

        builder.HasOne(x => x.Submission)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.SubmissionId
            })
            .HasConstraintName(
                "fk_submissionattachments_institutionsubmissions")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_submissionattachments_filename",
                "length(trim(filename)) > 0");

            table.HasCheckConstraint(
                "ck_submissionattachments_filepath",
                "length(trim(filepath)) > 0");

            table.HasCheckConstraint(
                "ck_submissionattachments_filesize",
                "filesize IS NULL OR filesize >= 0");
        });
    }
}