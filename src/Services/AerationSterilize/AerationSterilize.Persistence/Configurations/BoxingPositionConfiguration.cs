using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BoxingPositionConfiguration : IEntityTypeConfiguration<BoxingPosition>
{
    public void Configure(EntityTypeBuilder<BoxingPosition> builder)
    {
        builder.ToTable("BoxingPositions");
        builder.Property(x => x.PositionCode).HasMaxLength(10).IsRequired(true);

        builder.HasOne(x => x.BoxingColumn)
           .WithMany(x => x.BoxingPositions)
           .HasForeignKey(x => x.BoxingColumnId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Status)
            .HasConversion(status => status.Value, value => PositionStatus.FromValue(value));
    }
}
