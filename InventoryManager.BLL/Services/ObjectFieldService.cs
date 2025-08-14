using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Extensions;
using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class ObjectFieldService : BaseService<ObjectField>, IObjectFieldService
    {
        private readonly IPrivacyCheckerService _privacyCheckerService;

        public ObjectFieldService(IRepository<ObjectField> repository, IPrivacyCheckerService privacyCheckerService) : base(repository)
        {
            _privacyCheckerService = privacyCheckerService;
        }

        private async Task<BaseResponse<bool>> DataTypeCheck(ObjectFieldType type, string content)
        {
            var valid = false;
            switch (type)
            {
                case ObjectFieldType.@int:
                    valid = int.TryParse(content, out int intValue);
                    break;
                case ObjectFieldType.@float:
                    valid = float.TryParse(content, out float floatValue);
                    break;
                default:
                    valid = true;
                    break;
            }

            return new StandardResponse<bool>()
            {
                Data = valid,
                InnerStatusCode = valid ? InnerStatusCode.OK : InnerStatusCode.UnsupportedMediaType
            };
        }

        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<InventoryObject> _inventoryObjectRepository;

        public async Task<BaseResponse<ObjectField>> CreateMasterObjectField(MasterObjectFieldDTO masterObjectFieldDTO)
        {
            var attachObject = await _inventoryObjectRepository.GetOneWhereAsync(x => x.Id == masterObjectFieldDTO.InventoryObjectId && x.IsTemplate);
            if (attachObject == null)
            {
                return new StandardResponse<ObjectField>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }
            var IamAdmin = await _privacyCheckerService.IsAdmin(masterObjectFieldDTO.CreatorId);
            var IamInventoryOwner = await _privacyCheckerService.IsCreator<Inventory, InventoryCategory>(_inventoryRepository, attachObject.AttachedEntityId, masterObjectFieldDTO.CreatorId);

            if (IamAdmin.Data || IamInventoryOwner.Data.Success)
            {
                return await CreateEntityAsync(masterObjectFieldDTO.CreateEntity());
            }

            return new StandardResponse<ObjectField>()
            {
                InnerStatusCode = InnerStatusCode.Forbiden
            };
        }

        public async Task<BaseResponse<ObjectField>> CreateContentObjectField(ContentObjectFieldDTO contentObjectFieldDTO)
        {
            var masterObject = await _inventoryObjectRepository.GetOneWhereAsync(x => x.Id == contentObjectFieldDTO.InventoryObjectId && x.IsTemplate);
            if (masterObject == null)
            {
                return new StandardResponse<ObjectField>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }
            var accessToObject = await _privacyCheckerService.PrivacyCheckModifyInventoryObject(contentObjectFieldDTO.InventoryObjectId, contentObjectFieldDTO.CreatorId);
            if (accessToObject.Data)
            {
                var masterField = masterObject.ObjectFields.Where(x => x.Id == contentObjectFieldDTO.MasterFieldId).SingleOrDefault();
                if (masterField == null)
                {
                    return new StandardResponse<ObjectField>()
                    {
                        InnerStatusCode = InnerStatusCode.EntityNotFound
                    };
                }

                var newObjectField = contentObjectFieldDTO.CreateEntity(masterField);
                if ((await DataTypeCheck(newObjectField.Type, newObjectField.Content)).Data)
                {
                    return new StandardResponse<ObjectField>()
                    {
                        InnerStatusCode = InnerStatusCode.UnsupportedMediaType
                    };
                }

                return await CreateEntityAsync(newObjectField);
            }

            return new StandardResponse<ObjectField>()
            {
                InnerStatusCode = accessToObject.InnerStatusCode
            };
        }

        public async Task<BaseResponse<ObjectField>> UpdateMasterObjectField(UpdateMasterObjectFieldDTO updateMasterObjectFieldDTO)
        {
            var inventoryObject = await _inventoryObjectRepository.GetOneWhereAsync(x => x.Id == updateMasterObjectFieldDTO.InventoryObjectId && x.IsTemplate);
            var originalField = inventoryObject?.ObjectFields?.Where(x => x.Id == updateMasterObjectFieldDTO.Id).SingleOrDefault();
            if (inventoryObject == null || originalField == null)
            {
                return new StandardResponse<ObjectField>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }
            var IamAdmin = await _privacyCheckerService.IsAdmin(updateMasterObjectFieldDTO.CreatorId);
            var IsMyInventory = await _privacyCheckerService.IsCreator<Inventory, InventoryCategory>(_inventoryRepository, inventoryObject.AttachedEntityId, updateMasterObjectFieldDTO.CreatorId);
            if (IamAdmin.Data || IsMyInventory.Data.Success)
            {
                var response = await UpdateEntityAsync(x => x.Id == updateMasterObjectFieldDTO.Id, x => x
                    .SetProperty(x => x.Title, updateMasterObjectFieldDTO.Title ?? originalField.Title)
                    .SetProperty(x => x.Description, updateMasterObjectFieldDTO.Description ?? originalField.Description)
                    .SetProperty(x => x.Visible, updateMasterObjectFieldDTO.Visible ?? originalField.Visible)
                    .SetProperty(x => x.Type, updateMasterObjectFieldDTO.Type ?? originalField.Type)
                    , updateMasterObjectFieldDTO.Timestamp);

                var updatedField = await _repository.ReadOneWhereAsync(x => x.Id == updateMasterObjectFieldDTO.Id);

                return new StandardResponse<ObjectField>()
                {
                    Data = updatedField,
                    InnerStatusCode = response.InnerStatusCode
                };
            }

            return new StandardResponse<ObjectField>()
            {
                InnerStatusCode = InnerStatusCode.Forbiden
            };
        }

        public async Task<BaseResponse<ObjectField>> UpdateContentObjectField(UpdateContentObjectFieldDTO updateContentObjectFieldDTO)
        {
            var inventoryObject = await _inventoryObjectRepository.GetOneWhereAsync(x => x.Id == updateContentObjectFieldDTO.InventoryObjectId && !x.IsTemplate);
            var originalField = inventoryObject?.ObjectFields?.Where(x => x.Id == updateContentObjectFieldDTO.Id).SingleOrDefault();
            if (inventoryObject == null || originalField == null)
            {
                return new StandardResponse<ObjectField>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }
            var IamAdmin = await _privacyCheckerService.IsAdmin(updateContentObjectFieldDTO.CreatorId);
            var IsMyInventory = await _privacyCheckerService.IsCreator<Inventory, InventoryCategory>(_inventoryRepository, inventoryObject.AttachedEntityId, updateContentObjectFieldDTO.CreatorId);
            if (IamAdmin.Data || IsMyInventory.Data.Success || inventoryObject.CreatorId == updateContentObjectFieldDTO.CreatorId)
            {
                var response = await UpdateEntityAsync(x => x.Id == updateContentObjectFieldDTO.Id, x => x
                    .SetProperty(x => x.Content, updateContentObjectFieldDTO.Content ?? originalField.Content)
                    , updateContentObjectFieldDTO.Timestamp);

                var updatedField = await _repository.ReadOneWhereAsync(x => x.Id == updateContentObjectFieldDTO.Id);

                return new StandardResponse<ObjectField>()
                {
                    Data = updatedField,
                    InnerStatusCode = response.InnerStatusCode
                };
            }

            return new StandardResponse<ObjectField>()
            {
                InnerStatusCode = InnerStatusCode.Forbiden
            };
        }
    }
}
