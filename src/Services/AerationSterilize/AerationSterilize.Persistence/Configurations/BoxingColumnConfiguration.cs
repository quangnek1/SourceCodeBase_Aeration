using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BoxingColumnConfiguration : IEntityTypeConfiguration<BoxingColumn>
{
    public void Configure(EntityTypeBuilder<BoxingColumn> builder)
    {
        builder.ToTable("BoxingColumns");
        builder.Property(x => x.ColumnName).HasMaxLength(10).IsRequired(true);
    }
}
