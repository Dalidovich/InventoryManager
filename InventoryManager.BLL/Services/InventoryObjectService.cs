using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Extensions;
using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class InventoryObjectService : BaseService<InventoryObject>, IInventoryObjectService
    {
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<Account> _accountRepository;

        public InventoryObjectService(IRepository<InventoryObject> repository, IRepository<Inventory> inventoryRepository, IRepository<Account> accountRepository) : base(repository)
        {
            _inventoryRepository = inventoryRepository;
            _accountRepository = accountRepository;
        }

        public async Task<BaseResponse<InventoryObject>> CreateInventoryObject(InventoryObjectDTO inventoryObjectDTO)
        {
            var lastSequenceId = (await _repository.ReadAllWhereAsync(x => x.AttachedEntityId == inventoryObjectDTO.InventoryId &&
                !x.IsTemplate)).Select(x => x.SequenceId).Max();
            var newInventoryObject = inventoryObjectDTO.CreateEntity();

            newInventoryObject.SequenceId = lastSequenceId + 1;

            var inventoryObject = await _repository.AddAsync(newInventoryObject);
            await _repository.SaveAsync();

            return new StandardResponse<InventoryObject>()
            {
                Data = inventoryObject,
                InnerStatusCode = InnerStatusCode.EntityCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteInventoryObject(Guid inventoryObjectId, Guid accountId)
        {
            var privacyCheck = await PrivacyCheck(inventoryObjectId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var deleted = await DeleteEntityAsync(x => x.Id == inventoryObjectId);
                return new StandardResponse<bool>()
                {
                    Data = deleted.Data,
                    InnerStatusCode = deleted.InnerStatusCode,
                };
            }

            return privacyCheck;
        }

        public async Task<BaseResponse<InventoryObject>> UpdateInventoryObjectTitle(Guid inventoryObjectId, Guid accountId, string newTitle)
        {
            var privacyCheck = await PrivacyCheck(inventoryObjectId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var updated = await UpdateEntityAsync(x => x.Id == inventoryObjectId, x => x.SetProperty(e => e.Title, newTitle));
                var inventoryObject = await _repository.ReadOneWhereAsync(x => x.Id == inventoryObjectId);

                return new StandardResponse<InventoryObject>()
                {
                    Data = inventoryObject,
                    InnerStatusCode = updated.InnerStatusCode,
                };
            }

            return new StandardResponse<InventoryObject>()
            {
                InnerStatusCode = privacyCheck.InnerStatusCode
            };
        }

        private async Task<BaseResponse<bool>> PrivacyCheck(Guid inventoryObjectId, Guid accountId)
        {
            var account = await _accountRepository.ReadOneWhereAsync(x => x.Id == accountId);
            var inventoryObject = await _repository.ReadOneWhereAsync(x => x.Id == inventoryObjectId);

            if (account == null || inventoryObject == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            var inventory = await _inventoryRepository.ReadOneWhereAsync(x => x.Id == inventoryObject.AttachedEntityId);
            if (inventory == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound,
                };
            }

            if (account.Role == AccountRole.Admin || inventory.CreatorId == accountId || inventoryObject.CreatorId == accountId)
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
