using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class InventoryObjectConfiguration : IEntityTypeConfiguration<InventoryObject>
    {
        public const string Table_name = "inventory_object";

        public void Configure(EntityTypeBuilder<InventoryObject> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_inventory_object_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.Title)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("title");

            builder.Property(e => e.CreatorId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_creator_id");

            builder.Property(e => e.AttachedEntityId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_inventory_id");

            builder.Property(e => e.SequenceId)
                   .HasColumnType(EntityDataTypes.Smallint)
                   .HasColumnName("sequence_id");

            builder.Property(e => e.IsTemplate)
                   .HasColumnType(EntityDataTypes.Boolean)
                   .HasColumnName("is_template");

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ObjectFields)
                .WithOne()
                .HasForeignKey(e => e.AttachedEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Tags)
                .WithOne()
                .HasForeignKey(e => e.AttachedEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Likes)
                .WithOne()
                .HasForeignKey(e => e.AttachedEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}