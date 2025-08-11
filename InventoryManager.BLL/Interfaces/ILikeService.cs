using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface ILikeService : IService<Like>
    {
        public Task<BaseResponse<Like>> CreateLike(Guid inventoryObjectId, Guid accountId);
    }
}
