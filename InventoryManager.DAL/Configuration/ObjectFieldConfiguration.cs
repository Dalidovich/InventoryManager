using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class ObjectFieldConfiguration : IEntityTypeConfiguration<ObjectField>
    {
        public const string Table_name = "object_field";

        public void Configure(EntityTypeBuilder<ObjectField> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_object_field_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.Title)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("title");

            builder.Property(e => e.Description)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("description");

            builder.Property(e => e.Visible)
                   .HasColumnType(EntityDataTypes.Boolean)
                   .HasColumnName("visible");

            builder.Property(e => e.CreatorId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_creator_id");

            builder.Property(e => e.AttachedEntityId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_inventory_object_id");

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}