using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class WorkTableAssignmentConfiguration : IEntityTypeConfiguration<WorkTableAssignment>
{
    public void Configure(EntityTypeBuilder<WorkTableAssignment> builder)
    {
        builder.ToTable("WorkTableAssignments");
    }
}

