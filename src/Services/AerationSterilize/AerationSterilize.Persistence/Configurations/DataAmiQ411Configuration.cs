using AerationSterilize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class DataAmiQ411Configuration : IEntityTypeConfiguration<DataAmiQ411>
{
    public void Configure(EntityTypeBuilder<DataAmiQ411> builder)
    {
        builder.ToTable("DataAmiQ411");

        builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired(true);
        builder.Property(x => x.ProductInformation).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.DrawingNumber).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.CatalogCode).HasMaxLength(100).IsRequired(true);
        builder.Property(x => x.Destination).HasMaxLength(50).IsRequired(true);
    }
}
