using AerationSterilize.Domain.Entities;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class AerationColumnConfiguration : IEntityTypeConfiguration<AerationColumn>
{
    public void Configure(EntityTypeBuilder<AerationColumn> builder)
    {
        builder.ToTable(TableNames.AerationColumn);
        builder.Property(x => x.ColumnName).HasMaxLength(10).IsRequired(true);

    }
}
