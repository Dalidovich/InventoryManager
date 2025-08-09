using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IAccessAccountToInventoryService : IService<AccessAccountToInventory>
    {
        public Task<BaseResponse<AccessAccountToInventory>> CreateAccessAccountToInventory(AccessAccountToInventory accessAccountToInventory);
        public Task<BaseResponse<bool>> DeleteAccessAccountToInventory(Guid id, Guid accountId);
    }
}
