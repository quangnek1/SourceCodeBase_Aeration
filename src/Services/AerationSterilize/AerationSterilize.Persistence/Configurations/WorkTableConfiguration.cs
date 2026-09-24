using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
internal class WorkTableConfiguration : IEntityTypeConfiguration<WorkTable>
{
    public void Configure(EntityTypeBuilder<WorkTable> builder)
    {
        builder.ToTable("WorkTables");


        builder.Property(x => x.Name).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.Description).HasMaxLength(100).IsRequired(false);
    }
}
