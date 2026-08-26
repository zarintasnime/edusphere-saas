using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations.Academic;

public sealed class StudentProfileConfiguration
    : BaseEntityConfiguration<StudentProfile>
{

    public override void Configure(
        EntityTypeBuilder<StudentProfile> builder)
    {

        base.Configure(builder);



        builder.ToTable("studentprofiles");



        // Primary Key

        builder.HasKey(x => x.StudentId)
            .HasName("studentprofiles_pkey");



        builder.Property(x => x.StudentId)

            .HasColumnName("studentid")

            .ValueGeneratedOnAdd();







        // Institution

        builder.Property(x => x.InstitutionId)

            .HasColumnName("institutionid")

            .IsRequired();







        // User

        builder.Property(x => x.UserId)

            .HasColumnName("userid")

            .IsRequired();







        // Student Code

        builder.Property(x => x.StudentCode)

            .HasColumnName("studentcode")

            .HasMaxLength(50)

            .IsRequired();







        // Admission Date

        builder.Property(x => x.AdmissionDate)

            .HasColumnName("admissiondate")

            .HasColumnType("date");







        // Active Status

        builder.Property(x => x.IsActive)

            .HasColumnName("isactive")

            .HasColumnType("boolean")

            .HasDefaultValue(true);







        // Updated At

        builder.Property(x => x.UpdatedAt)

            .HasColumnName("updatedat")

            .HasColumnType(
                "timestamp without time zone"
            );









        // Institution Relationship


        builder.HasOne(x => x.Institution)

            .WithMany()

            .HasForeignKey(x => x.InstitutionId)

            .HasConstraintName(
                "fk_studentprofiles_institutions"
            )

            .OnDelete(DeleteBehavior.Cascade);









        // User Relationship


        builder.HasOne(x => x.User)

            .WithOne()

            .HasPrincipalKey<User>(x => new
            {
                x.InstitutionId,
                x.UserId
            })

            .HasForeignKey<StudentProfile>(x => new
            {
                x.InstitutionId,
                x.UserId
            })

            .HasConstraintName(
                "fk_studentprofiles_institutionusers"
            )

            .OnDelete(DeleteBehavior.Restrict);









        // Unique User per Institution


        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.UserId

        })

        .IsUnique()

        .HasDatabaseName(
            "uq_studentprofiles_institution_user"
        );









        // Unique Student Code per Institution


        builder.HasIndex(x => new
        {
            x.InstitutionId,
            x.StudentCode

        })

        .IsUnique()

        .HasDatabaseName(
            "uq_studentprofiles_institution_studentcode"
        );









        // Alternate Key


        builder.HasAlternateKey(x => new
        {
            x.InstitutionId,
            x.StudentId

        })

        .HasName(
            "uq_studentprofiles_institution_student"
        );









        // Check Constraint


        builder.ToTable(table =>
        {

            table.HasCheckConstraint(

                "ck_studentprofiles_studentcode",

                "length(trim(studentcode)) > 0"

            );

        });

    }

}