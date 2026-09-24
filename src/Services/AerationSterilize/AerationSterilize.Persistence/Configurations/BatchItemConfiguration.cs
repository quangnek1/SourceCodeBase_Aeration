using AerationSterilize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BatchItemConfiguration : IEntityTypeConfiguration<BatchItem>
{
    public void Configure(EntityTypeBuilder<BatchItem> builder)
    {
        builder.ToTable("BatchItem");

        builder.HasOne(x => x.Batch)
            .WithMany(x => x.BatchItems)
            .HasForeignKey(x => x.BatchId);
    }
}
