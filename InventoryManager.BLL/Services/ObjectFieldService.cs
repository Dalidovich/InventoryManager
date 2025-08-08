using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    internal class ObjectFieldService : BaseService<ObjectField>, IObjectFieldService
    {
        public ObjectFieldService(IRepository<ObjectField> repository) : base(repository)
        {
        }

        public async Task<BaseResponse<ObjectField>> CreateObjectFieldAsync(ObjectField objectField)
        {
            var valid = false;
            switch (objectField.Type)
            {
                case Domain.Enums.ObjectFieldType.@int:
                    valid = int.TryParse(objectField.Content, out int intValue);
                    break;
                case Domain.Enums.ObjectFieldType.@float:
                    valid = float.TryParse(objectField.Content, out float floatValue);
                    break;
                default:
                    valid = true;
                    break;
            }
            if (valid)
            {
                var newObjectFieldResponse = await CreateEntityAsync(objectField);
                if (newObjectFieldResponse.InnerStatusCode == InnerStatusCode.EntityCreate)
                {
                    return new StandardResponse<ObjectField>()
                    {
                        Data = newObjectFieldResponse.Data,
                        InnerStatusCode = newObjectFieldResponse.InnerStatusCode,
                    };
                }
            }

            return new StandardResponse<ObjectField>()
            {
                Data = null,
                InnerStatusCode = InnerStatusCode.UnsupportedMediaType
            };
        }
    }
}
