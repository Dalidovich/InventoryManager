namespace InventoryManager.BLL.DTO
{
    public class InventoryObjectDTO
    {
        public string Title { get; set; }
        public Guid InventoryId { get; set; }
        public Guid CreatorId { get; set; }
        public bool IsTemplate { get; set; }
    }
}
