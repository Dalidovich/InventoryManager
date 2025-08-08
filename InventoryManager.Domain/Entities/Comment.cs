namespace InventoryManager.Domain.Entities
{
    public class Comment : AttachedToEntity<Inventory>
    {
        public string Content { get; set; }

        public virtual Account? Author { get; set; }
    }
}
