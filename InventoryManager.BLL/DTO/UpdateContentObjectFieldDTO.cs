namespace InventoryManager.BLL.DTO
{
    public class UpdateContentObjectFieldDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid InventoryObjectId { get; set; }
    }
}
