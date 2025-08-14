using InventoryManager.Domain.Enums;

namespace InventoryManager.BLL.DTO
{
    public class MasterObjectFieldDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
        public ObjectFieldType Type { get; set; }
        public Guid InventoryObjectId { get; set; }
        public Guid CreatorId { get; set; }
    }
}
