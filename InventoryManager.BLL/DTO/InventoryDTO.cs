namespace InventoryManager.BLL.DTO
{
    public class InventoryDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgURL { get; set; }
        public Guid CreatorId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
