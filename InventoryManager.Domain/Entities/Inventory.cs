using InventoryManager.Domain.Enums;

namespace InventoryManager.Domain.Entities
{
    public class Inventory : AttachedToEntity<InventoryCategory>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public InventoryState State { get; set; }
        public string ImgURL { get; set; }

        public virtual Account? Creator { get; set; }
        public virtual List<InventoryObject>? InventoryObjects { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual InventoryCategory? Category { get; set; }

        public Inventory() : base()
        {

        }
    }
}
