using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("Batch");

        builder.Property(x => x.QRCode).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.BatchNo).HasMaxLength(10).IsRequired(true);

    }
}
