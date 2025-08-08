using InventoryManager.DAL.Configuration.DataType;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManager.DAL.Configuration
{
    public class TokenDataConfiguration : IEntityTypeConfiguration<TokenData>
    {
        public const string Table_name = "token_data";

        public void Configure(EntityTypeBuilder<TokenData> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.AccountId });
            builder.HasIndex(e => e.RefreshToken);

            builder.Property(e => e.AccountId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("pk_token_data_account_id");

            builder.Property(e => e.RefreshToken)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("refresh_token");
        }
    }
}
