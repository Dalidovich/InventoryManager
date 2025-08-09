using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class InventoryService : BaseService<Inventory>, IInventoryService
    {
        private readonly IRepository<Account> _accountRepository;

        public InventoryService(IRepository<Inventory> repository, IRepository<Account> accountRepository) : base(repository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<BaseResponse<bool>> DeleteInventory(Guid inventoryId, Guid accountId)
        {
            var privacyCheck = await PrivacyCheck(inventoryId, accountId);
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
            var privacyCheck = await PrivacyCheck(inventoryId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var updated = await UpdateEntityAsync(x => x.Id == inventoryId, x => x.SetProperty(e => e.State, newInventoryState));
                var inventory = await _repository.ReadOneWhereAsync(x => x.Id == inventoryId);

                return new StandardResponse<Inventory>()
                {
                    Data = inventory,
                    InnerStatusCode = updated.InnerStatusCode,
                };
            }

            return new StandardResponse<Inventory>()
            {
                InnerStatusCode = privacyCheck.InnerStatusCode
            };
        }

        private async Task<BaseResponse<bool>> PrivacyCheck(Guid inventoryId, Guid accountId)
        {
            var inventory = await _repository.ReadOneWhereAsync(x => x.Id == inventoryId);
            var account = await _accountRepository.ReadOneWhereAsync(x => x.Id == accountId);
            if (inventory == null || account == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            if (account.Role == AccountRole.Admin || inventory.CreatorId == accountId)
            {
                return new StandardResponse<bool>()
                {
                    Data = true,
                    InnerStatusCode = InnerStatusCode.AccountAuthenticate,
                };
            }

            return new StandardResponse<bool>()
            {
                Data = false,
                InnerStatusCode = InnerStatusCode.Forbiden,
            };
        }
    }
}
