using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public const string Table_name = "like";

        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_like_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.CreatorId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_creator_id");

            builder.Property(e => e.AttachedEntityId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_inventory_id");
        }
    }
}