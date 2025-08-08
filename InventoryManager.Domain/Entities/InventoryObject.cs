namespace InventoryManager.Domain.Entities
{
    public class InventoryObject : AttachedToEntity<Inventory>
    {
        public string Title { get; set; }
        public int SequenceId { get; set; }
        public bool IsTemplate { get; set; }

        public virtual Account? Creator { get; set; }
        public virtual List<ObjectField>? ObjectFields { get; set; }
        public virtual List<Tag>? Tags { get; set; }
        public virtual List<Like>? Likes { get; set; }

        public InventoryObject() : base()
        {

        }
    }
}
