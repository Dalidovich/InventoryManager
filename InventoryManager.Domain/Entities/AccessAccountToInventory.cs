namespace InventoryManager.Domain.Entities
{
    public class AccessAccountToInventory : AttachedToEntity<Inventory>
    {
        public virtual Account? SlaveAccount { get; set; }
        public virtual Account? MasterAccount { get; set; }
        public virtual Inventory? Inventory { get; set; }
    }
}
