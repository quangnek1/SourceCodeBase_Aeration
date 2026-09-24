using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class AerationPositionConfiguration : IEntityTypeConfiguration<AerationPosition>
{
    public void Configure(EntityTypeBuilder<AerationPosition> builder)
    {
        builder.ToTable("AerationPosition");
        builder.Property(x => x.PositionCode).HasMaxLength(10).IsRequired(true);

        builder.HasOne(x => x.AerationColumn)
           .WithMany(x => x.AerationPositions)
           .HasForeignKey(x => x.AerationColumnId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Status)
            .HasConversion(
            status => status.Value,
            value => AerationStatus.FromValue(value));
    }
}
