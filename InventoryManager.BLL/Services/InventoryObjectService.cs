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
        private readonly IPrivacyCheckerService _privacyCheckerService;

        public InventoryObjectService(IRepository<InventoryObject> repository, IPrivacyCheckerService privacyCheckerService) : base(repository)
        {
            _privacyCheckerService = privacyCheckerService;
        }

        public async Task<BaseResponse<InventoryObject>> CreateInventoryObject(InventoryObjectDTO inventoryObjectDTO)
        {
            var lastSequenceId = (await ReadEntitiesAsync(x => x.AttachedEntityId == inventoryObjectDTO.InventoryId &&
                !x.IsTemplate)).Data.Select(x => x.SequenceId).Max();
            var newInventoryObject = inventoryObjectDTO.CreateEntity();

            newInventoryObject.SequenceId = lastSequenceId + 1;

            var inventoryObject = await CreateEntityAsync(newInventoryObject);

            return new StandardResponse<InventoryObject>()
            {
                Data = inventoryObject.Data,
                InnerStatusCode = inventoryObject.InnerStatusCode
            };
        }

        public async Task<BaseResponse<bool>> DeleteInventoryObject(Guid inventoryObjectId, Guid accountId)
        {
            var privacyCheck = await _privacyCheckerService.PrivacyCheckInventoryObject(inventoryObjectId, accountId);
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
            var privacyCheck = await _privacyCheckerService.PrivacyCheckInventoryObject(inventoryObjectId, accountId);
            if (privacyCheck.InnerStatusCode == InnerStatusCode.AccountAuthenticate)
            {
                var updated = await UpdateEntityAsync(x => x.Id == inventoryObjectId, x => x.SetProperty(e => e.Title, newTitle));
                var inventoryObjectI = await ReadEntityAsync(x => x.Id == inventoryObjectId);

                return new StandardResponse<InventoryObject>()
                {
                    Data = inventoryObjectI.Data,
                    InnerStatusCode = updated.InnerStatusCode,
                };
            }

            return new StandardResponse<InventoryObject>()
            {
                InnerStatusCode = privacyCheck.InnerStatusCode
            };
        }
    }
}
