namespace InventoryManager.BLL.DTO
{
    public class ContentObjectFieldDTO
    {
        public Guid MasterFieldId { get; set; }
        public string Content { get; set; }
        public Guid InventoryObjectId { get; set; }
        public Guid CreatorId { get; set; }
    }
}
