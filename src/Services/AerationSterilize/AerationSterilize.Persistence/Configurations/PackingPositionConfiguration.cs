using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Enums;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class PackingPositionConfiguration : IEntityTypeConfiguration<PackingPosition>
{
    public void Configure(EntityTypeBuilder<PackingPosition> builder)
    {
        builder.ToTable(TableNames.PackingPosition);
        builder.Property(x => x.PositionCode).HasMaxLength(10).IsRequired(true);

        builder.HasOne(x => x.PackingColumn)
           .WithMany(x => x.PackingPositions)
           .HasForeignKey(x => x.PackingColumnId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Status)
            .HasConversion(status => status.Value, value => AerationStatus.FromValue(value));
    }
}
