using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public const string Table_name = "comment";

        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Id)
                  .HasColumnType(EntityDataTypes.Guid)
                  .HasColumnName("pk_comment_id");

            builder.Property(e => e.CreatedAt)
                  .HasColumnName("create_at");

            builder.Property(e => e.Timestamp)
                  .HasColumnName("timestamp");

            builder.Property(e => e.Content)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("content");

            builder.Property(e => e.AttachedEntityId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_inventory_id");

            builder.Property(e => e.CreatorId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_creator_id");

            builder.HasOne(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}