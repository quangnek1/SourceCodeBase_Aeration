using AerationSterilize.Domain.Entities;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class PackingColumnConfiguration : IEntityTypeConfiguration<PackingColumn>
{
    public void Configure(EntityTypeBuilder<PackingColumn> builder)
    {
        builder.ToTable(TableNames.PackingColumn);
        builder.Property(x => x.ColumnName).HasMaxLength(10).IsRequired(true);

    }
}
