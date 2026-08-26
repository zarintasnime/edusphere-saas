using AssignmentSubmissionManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreatedAt)
            .HasColumnName("createdat")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}