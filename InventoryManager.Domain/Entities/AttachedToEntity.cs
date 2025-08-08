namespace InventoryManager.Domain.Entities
{
    public class AttachedToEntity<T> : BaseEntity
    {
        public Guid AttachedEntityId { get; set; }
        public Guid CreatorId { get; set; }
    }
}
