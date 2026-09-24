using AerationSterilize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
public class DataPlanConfiguration : IEntityTypeConfiguration<DataPlan>
{
    public void Configure(EntityTypeBuilder<DataPlan> builder)
    {
        builder.ToTable("DataPlan");

        builder.Property(x => x.PONo).HasMaxLength(20);
        builder.Property(x => x.TT).HasMaxLength(20);
        builder.Property(x => x.MaterialTypeC).HasMaxLength(20);
        builder.Property(x => x.PldOrd).HasMaxLength(20);
        builder.Property(x => x.Material).HasMaxLength(20);
        builder.Property(x => x.ItemCodeTypeC).HasMaxLength(20);
        builder.Property(x => x.ItemCode).HasMaxLength(20);
        builder.Property(x => x.DrawingListNo).HasMaxLength(50);
        builder.Property(x => x.ItemName).HasMaxLength(100);
        builder.Property(x => x.InternalLot).HasMaxLength(20);
        builder.HasIndex(x => x.InternalLot).IsUnique();
        builder.Property(x => x.Phase).HasMaxLength(20);
        builder.Property(x => x.ExternalLot1).HasMaxLength(20);

        builder.Property(x => x.Qty).HasDefaultValue(0);
        builder.Property(x => x.QtyInput).HasDefaultValue(0);
        builder.Property(x => x.KeepAeration).HasDefaultValue(0);

        builder.Property(x => x.Destination).HasMaxLength(20);
        builder.Property(x => x.Bioburden).HasMaxLength(20);
        builder.Property(x => x.ME).HasMaxLength(10);
        builder.Property(x => x.BOXING).HasMaxLength(25);
        builder.Property(x => x.TestEndotoxin).HasMaxLength(20);
        builder.Property(x => x.TestParticle).HasMaxLength(20);
        builder.Property(x => x.MEChia).HasMaxLength(20);


    }
}
