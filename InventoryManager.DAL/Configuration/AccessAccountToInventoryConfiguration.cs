using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class AccessAccountToInventoryConfiguration : IEntityTypeConfiguration<AccessAccountToInventory>
    {
        public const string Table_name = "access_account_to_inventory";

        public void Configure(EntityTypeBuilder<AccessAccountToInventory> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id, e.AttachedEntityId });

            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_slave_account_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.AttachedEntityId)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_inventory_id");

            builder.Property(e => e.CreatorId)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("fk_master_account_id");

            builder.HasOne(x => x.MasterAccount)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SlaveAccount)
                .WithMany()
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Inventory)
                .WithMany()
                .HasForeignKey(x => x.AttachedEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}