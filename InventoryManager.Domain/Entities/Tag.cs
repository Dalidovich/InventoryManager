namespace InventoryManager.Domain.Entities
{
    public class Tag : AttachedToEntity<InventoryObject>
    {
        public string Title { get; set; }
    }
}
