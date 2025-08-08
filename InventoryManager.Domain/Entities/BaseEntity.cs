namespace InventoryManager.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Timestamp { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
