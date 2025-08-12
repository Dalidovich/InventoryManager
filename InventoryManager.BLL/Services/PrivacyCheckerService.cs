using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class PrivacyCheckerService : IPrivacyCheckerService
    {
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<AccessAccountToInventory> _accessAccountToInventoryRepository;
        private readonly IRepository<InventoryObject> _inventoryObjectRepository;
        private readonly IRepository<Account> _accountRepository;

        public PrivacyCheckerService(IRepository<Inventory> inventoryRepository, IRepository<InventoryObject> inventoryObjectRepository, IRepository<Account> accountRepository, IRepository<AccessAccountToInventory> accessAccountToInventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryObjectRepository = inventoryObjectRepository;
            _accountRepository = accountRepository;
            _accessAccountToInventoryRepository = accessAccountToInventoryRepository;
        }

        public async Task<BaseResponse<PrivacyCreatorResult>> IsCreator<TEntity, TAttach>(IRepository<TEntity> repo, Guid entityId, Guid accountId) where TEntity : AttachedToEntity<TAttach>
        {
            var entity = await repo.GetOneWhereAsync(x => x.Id == entityId);
            if (entity == null)
            {
                return new StandardResponse<PrivacyCreatorResult>()
                {
                    Data = new(false),
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            return new StandardResponse<PrivacyCreatorResult>()
            {
                Data = new(entity.CreatorId == accountId, entity.AttachedEntityId),
                InnerStatusCode = entity.CreatorId == accountId ? InnerStatusCode.AccountAuthenticate : InnerStatusCode.Forbiden,
            };
        }

        public async Task<BaseResponse<bool>> IsAccessToInventory(Guid inventoryId, Guid accountId)
        {
            var entity = await _accessAccountToInventoryRepository.GetOneWhereAsync(x => x.Id == accountId && x.AttachedEntityId == inventoryId);
            if (entity == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            return new StandardResponse<bool>()
            {
                Data = true,
                InnerStatusCode = entity.CreatorId == accountId ? InnerStatusCode.AccountAuthenticate : InnerStatusCode.Forbiden,
            };
        }

        public async Task<BaseResponse<bool>> IsAdmin(Guid accountId)
        {
            var account = await _accountRepository.ReadOneWhereAsync(x => x.Id == accountId);
            if (account == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            return new StandardResponse<bool>()
            {
                Data = account.Role == AccountRole.Admin,
                InnerStatusCode = account.Role == AccountRole.Admin ? InnerStatusCode.AccountAuthenticate : InnerStatusCode.Forbiden,
            };
        }

        public async Task<BaseResponse<bool>> PrivacyCheckInventory(Guid inventoryId, Guid accountId)
        {
            var privacyAdmin = await IsAdmin(accountId);
            var privacyCreator = await IsCreator<Inventory, InventoryCategory>(_inventoryRepository, inventoryId, accountId);

            if (privacyAdmin.Data || privacyCreator.Data.Success)
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

        public async Task<BaseResponse<bool>> PrivacyCheckModifyInventoryObject(Guid inventoryObjectId, Guid accountId)
        {
            var privacyAdmin = await IsAdmin(accountId);
            var privacyInventoryObjectCreator = await IsCreator<InventoryObject, Inventory>(_inventoryObjectRepository, inventoryObjectId, accountId);
            if (privacyInventoryObjectCreator.InnerStatusCode == InnerStatusCode.EntityNotFound)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = privacyInventoryObjectCreator.InnerStatusCode,
                };
            }
            var privacyInventoryCreator = await IsCreator<Inventory, InventoryCategory>(_inventoryRepository,
                    (Guid)privacyInventoryObjectCreator.Data.AttachedId, accountId);

            if (privacyAdmin.Data || privacyInventoryCreator.Data.Success || privacyInventoryObjectCreator.Data.Success)
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
