using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BoxConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> builder)
    {
        builder.ToTable("Boxes");

        builder.Property(x => x.BoxCode).HasMaxLength(100).IsRequired(true);
        builder.Property(x => x.INT).HasMaxLength(100).IsRequired(true);
        builder.Property(x => x.SEQ).HasMaxLength(100);
    }
}
