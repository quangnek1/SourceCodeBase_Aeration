using AerationSterilize.Domain.Entities;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class SettingsConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable(TableNames.Settings);
        builder.Property(x => x.PlanPTCA).HasMaxLength(150).IsRequired(true);
        builder.Property(x => x.PlanCAG).HasMaxLength(150).IsRequired(true);
        builder.Property(x => x.DataAmiQ411).HasMaxLength(150).IsRequired(true);
    }
}
