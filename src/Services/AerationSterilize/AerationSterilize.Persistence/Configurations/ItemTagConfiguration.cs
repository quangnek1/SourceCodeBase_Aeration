using AerationSterilize.Domain.Entities.Working;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AerationSterilize.Persistence.Configurations;
internal class ItemTagConfiguration : IEntityTypeConfiguration<ItemTag>
{
    public void Configure(EntityTypeBuilder<ItemTag> builder)
    {
        builder.ToTable("ItemTags");

        builder.Property(x => x.ItemcD).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.Ext1).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.Ext2).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.SEQNo).HasMaxLength(50).IsRequired(true);
    }
}
