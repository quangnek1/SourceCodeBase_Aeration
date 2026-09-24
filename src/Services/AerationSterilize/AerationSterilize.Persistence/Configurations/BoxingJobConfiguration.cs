using AerationSterilize.Domain.Entities.Working;
using AerationSterilize.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class BoxingJobConfiguration : IEntityTypeConfiguration<BoxingJob>
{
    public void Configure(EntityTypeBuilder<BoxingJob> builder)
    {
        builder.ToTable("BoxingJobs");
    }
}
