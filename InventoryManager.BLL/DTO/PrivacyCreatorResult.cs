namespace InventoryManager.BLL.DTO
{
    public class PrivacyCreatorResult
    {
        public bool Success { get; set; }
        public Guid? AttachedId { get; set; }

        public PrivacyCreatorResult(bool success, Guid? id = null) 
        {
            Success = success;
            AttachedId = id;
        }
    }
}
