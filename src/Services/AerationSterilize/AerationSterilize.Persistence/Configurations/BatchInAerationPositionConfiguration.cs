using AerationSterilize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BatchInAerationPositionConfiguration : IEntityTypeConfiguration<BatchInAerationPosition>
{
    public void Configure(EntityTypeBuilder<BatchInAerationPosition> builder)
    {
        builder.ToTable("BatchInAerationPosition");

        builder.HasOne(x => x.Batch)
            .WithMany(x => x.BatchInAerationPositions)
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AerationPosition)
            .WithMany(x => x.BatchInAerationPositions)
            .HasForeignKey(x => x.AerationPositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.BatchId,
            x.AerationPositionId
        }).IsUnique(false);
    }
}
