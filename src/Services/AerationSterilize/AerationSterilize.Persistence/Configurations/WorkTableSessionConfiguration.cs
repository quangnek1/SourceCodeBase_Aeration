using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
internal class WorkTableSessionConfiguration : IEntityTypeConfiguration<WorkTableSession>
{
    public void Configure(EntityTypeBuilder<WorkTableSession> builder)
    {
        builder.ToTable("WorkTableSessions");
    }
}
