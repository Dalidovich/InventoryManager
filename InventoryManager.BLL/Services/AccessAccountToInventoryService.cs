using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class AccessAccountToInventoryService : BaseService<AccessAccountToInventory>, IAccessAccountToInventoryService
    {
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<Account> _accountRepository;

        public AccessAccountToInventoryService(IRepository<AccessAccountToInventory> repository, IRepository<Account> accountService, IRepository<Inventory> inventoryService) : base(repository)
        {
            _accountRepository = accountService;
            _inventoryRepository = inventoryService;
        }

        public async Task<BaseResponse<AccessAccountToInventory>> CreateAccessAccountToInventory(AccessAccountToInventory accessAccountToInventory)
        {
            var creator = await _accountRepository.ReadOneWhereAsync(x => x.Id == accessAccountToInventory.CreatorId);
            if (creator == null)
            {
                return new StandardResponse<AccessAccountToInventory>()
                {
                    InnerStatusCode = InnerStatusCode.Forbiden
                };
            }
            if (creator.Role == AccountRole.Admin)
            {
                var access = await CreateEntityAsync(accessAccountToInventory);

                return new StandardResponse<AccessAccountToInventory>()
                {
                    Data = access.Data,
                    InnerStatusCode = access.InnerStatusCode
                };
            }
            else
            {
                var ownInventory = await _inventoryRepository
                    .ReadOneWhereAsync(x => x.Id == accessAccountToInventory.AttachedEntityId && x.CreatorId == creator.Id);
                if (ownInventory == null)
                {
                    return new StandardResponse<AccessAccountToInventory>()
                    {
                        InnerStatusCode = InnerStatusCode.Forbiden
                    };
                }

                var access = await CreateEntityAsync(accessAccountToInventory);

                return new StandardResponse<AccessAccountToInventory>()
                {
                    Data = access.Data,
                    InnerStatusCode = access.InnerStatusCode
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAccessAccountToInventory(Guid id, Guid accountId)
        {
            var creator = await _accountRepository.ReadOneWhereAsync(x => x.Id == accountId);
            if (creator == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }
            var deleted = await DeleteEntityAsync(x => x.Id == id && (x.Inventory.CreatorId == creator.Id || creator.Role == AccountRole.Admin));

            return new StandardResponse<bool>()
            {
                Data = deleted.Data,
                InnerStatusCode = deleted.Data ? InnerStatusCode.EntityDelete : InnerStatusCode.Forbiden
            };
        }
    }
}
