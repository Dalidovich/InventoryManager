using InventoryManager.BLL.DTO;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IInventoryObjectService : IService<InventoryObject>
    {
        public Task<BaseResponse<InventoryObject>> CreateInventoryObject(InventoryObjectDTO inventoryObjectDTO);
        public Task<BaseResponse<bool>> DeleteInventoryObject(Guid inventoryObjectId, Guid accountId);
        public Task<BaseResponse<InventoryObject>> UpdateInventoryObjectTitle(Guid inventoryObjectId, Guid accountId, string newTitle, DateTime timestamp);
    }
}
