using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class InventoryCategoryConfiguration : IEntityTypeConfiguration<InventoryCategory>
    {
        public const string Table_name = "inventory_category";

        public void Configure(EntityTypeBuilder<InventoryCategory> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });


            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_inventory_category_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.Name)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("name");
        }
    }
}