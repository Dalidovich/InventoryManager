using InventoryManager.Domain.Enums;

namespace InventoryManager.Domain.Entities
{
    public class ObjectField : AttachedToEntity<InventoryObject>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }

        public string Content { get; set; }
        public ObjectFieldType Type { get; set; }

        public virtual Account? Creator { get; set; }
    }
}
