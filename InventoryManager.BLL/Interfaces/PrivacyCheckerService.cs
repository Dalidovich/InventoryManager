using InventoryManager.BLL.DTO;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IPrivacyCheckerService
    {
        public Task<BaseResponse<PrivacyCreatorResult>> IsCreator<TEntity, TAttach>(IRepository<TEntity> repo, Guid entityId, Guid accountId) where TEntity : AttachedToEntity<TAttach>;
        public Task<BaseResponse<bool>> IsAdmin(Guid accountId);
        public Task<BaseResponse<bool>> IsAccessToInventory(Guid inventoryId, Guid accountId);
        public Task<BaseResponse<bool>> PrivacyCheckInventory(Guid inventoryId, Guid accountId);
        public Task<BaseResponse<bool>> PrivacyCheckModifyInventoryObject(Guid inventoryObjectId, Guid accountId);
    }
}
