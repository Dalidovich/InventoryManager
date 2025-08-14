using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IInventoryService : IService<Inventory>
    {
        public Task<BaseResponse<Inventory>> UpdateInventoryState(Guid inventoryId, Guid accountId, InventoryState newInventoryState, DateTime timestamp);
        public Task<BaseResponse<bool>> DeleteInventory(Guid inventoryId, Guid accountId);
    }
}
