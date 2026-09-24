using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BoxingSessionConfiguration : IEntityTypeConfiguration<BoxingSession>
{
    public void Configure(EntityTypeBuilder<BoxingSession> builder)
    {
        builder.ToTable("BoxingSessions");
        builder.HasKey(x => x.Id);
    }
}
