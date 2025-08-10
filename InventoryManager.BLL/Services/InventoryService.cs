using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class InventoryService : BaseService<Inventory>, IInventoryService
    {
        private readonly IPrivacyCheckerService _privacyCheckerService;

        public InventoryService(IRepository<Inventory> repository, IPrivacyCheckerService privacyCheckerService) : base(repository)
        {
            _privacyCheckerService = privacyCheckerService;
        }

        public async Task<BaseResponse<bool>> DeleteInventory(Guid inventoryId, Guid accountId)
        {
            var privacyCheck = await _privacyCheckerService.PrivacyCheckInventory(inventoryId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var deleted = await DeleteEntityAsync(x => x.Id == inventoryId);
                return new StandardResponse<bool>()
                {
                    Data = deleted.Data,
                    InnerStatusCode = deleted.InnerStatusCode,
                };
            }

            return privacyCheck;
        }

        public async Task<BaseResponse<Inventory>> UpdateInventoryState(Guid inventoryId, Guid accountId, InventoryState newInventoryState)
        {
            var privacyCheck = await _privacyCheckerService.PrivacyCheckInventory(inventoryId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var updated = await UpdateEntityAsync(x => x.Id == inventoryId, x => x.SetProperty(e => e.State, newInventoryState));
                var inventory = await ReadEntityAsync(x => x.Id == inventoryId);

                return new StandardResponse<Inventory>()
                {
                    Data = inventory.Data,
                    InnerStatusCode = updated.InnerStatusCode,
                };
            }

            return new StandardResponse<Inventory>()
            {
                InnerStatusCode = privacyCheck.InnerStatusCode
            };
        }
    }
}
