using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.AssignmentManagement;

public sealed class AssignmentAttachmentConfiguration
    : BaseEntityConfiguration<AssignmentAttachment>
{
    public override void Configure(
        EntityTypeBuilder<AssignmentAttachment> builder)
    {
        base.Configure(builder);

        builder.ToTable("assignmentattachments");

        builder.HasKey(x => x.AttachmentId)
            .HasName("assignmentattachments_pkey");

        builder.Property(x => x.AttachmentId)
            .HasColumnName("attachmentid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstitutionId)
            .HasColumnName("institutionid")
            .IsRequired();

        builder.Property(x => x.AssignmentId)
            .HasColumnName("assignmentid")
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

        builder.HasOne(x => x.Assignment)
            .WithMany()
            .HasPrincipalKey(x => new
            {
                x.InstitutionId,
                x.AssignmentId
            })
            .HasForeignKey(x => new
            {
                x.InstitutionId,
                x.AssignmentId
            })
            .HasConstraintName(
                "fk_assignmentattachments_institutionassignments")
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_assignmentattachments_filename",
                "length(trim(filename)) > 0");

            table.HasCheckConstraint(
                "ck_assignmentattachments_filepath",
                "length(trim(filepath)) > 0");

            table.HasCheckConstraint(
                "ck_assignmentattachments_filesize",
                "filesize IS NULL OR filesize >= 0");
        });
    }
}