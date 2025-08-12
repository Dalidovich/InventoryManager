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

        private async Task<BaseResponse<bool>> DataTypeCheck(ObjectField objectField)
        {
            var valid = false;
            switch (objectField.Type)
            {
                case ObjectFieldType.@int:
                    valid = int.TryParse(objectField.Content, out int intValue);
                    break;
                case ObjectFieldType.@float:
                    valid = float.TryParse(objectField.Content, out float floatValue);
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
    }
}
