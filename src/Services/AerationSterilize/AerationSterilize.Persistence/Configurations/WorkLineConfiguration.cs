using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
internal class WorkLineConfiguration : IEntityTypeConfiguration<WorkLine>
{
    public void Configure(EntityTypeBuilder<WorkLine> builder)
    {
        builder.ToTable("WorkLines");


        builder.Property(x => x.Name).HasMaxLength(50).IsRequired(true);
    }
}
