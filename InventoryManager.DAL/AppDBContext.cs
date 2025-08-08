using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventoryManager.DAL
{
    public class AppDBContext : DbContext
    {
        public DbSet<AccessAccountToInventory> AccessAccountToInventorys { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Inventory> Inventorys { get; set; }
        public DbSet<InventoryCategory> InventoryCategorys { get; set; }
        public DbSet<InventoryObject> InventoryObjects { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ObjectField> ObjectFields { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TokenData> TokenDatas { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public AppDBContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<T> GetTable<T>() where T : class
        {
            return typeof(T) switch
            {
                Type t when t == typeof(AccessAccountToInventory) => (DbSet<T>)(object)AccessAccountToInventorys,
                Type t when t == typeof(Account) => (DbSet<T>)(object)Accounts,
                Type t when t == typeof(Comment) => (DbSet<T>)(object)Comments,
                Type t when t == typeof(Inventory) => (DbSet<T>)(object)Inventorys,
                Type t when t == typeof(InventoryCategory) => (DbSet<T>)(object)InventoryCategorys,
                Type t when t == typeof(InventoryObject) => (DbSet<T>)(object)InventoryObjects,
                Type t when t == typeof(Like) => (DbSet<T>)(object)Likes,
                Type t when t == typeof(ObjectField) => (DbSet<T>)(object)ObjectFields,
                Type t when t == typeof(Tag) => (DbSet<T>)(object)Tags,
                Type t when t == typeof(TokenData) => (DbSet<T>)(object)TokenDatas,
                _ => throw new ArgumentException($"No DbSet found for type {typeof(T).Name}")
            };
        }
    }
}