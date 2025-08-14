using InventoryManager.BLL.DTO;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IObjectFieldService : IService<ObjectField>
    {
        public Task<BaseResponse<ObjectField>> CreateMasterObjectField(MasterObjectFieldDTO masterObjectFieldDTO);
        public Task<BaseResponse<ObjectField>> CreateContentObjectField(ContentObjectFieldDTO contentObjectFieldDTO);
        public Task<BaseResponse<ObjectField>> UpdateMasterObjectField(UpdateMasterObjectFieldDTO masterObjectFieldDTO);
        public Task<BaseResponse<ObjectField>> UpdateContentObjectField(UpdateContentObjectFieldDTO contentObjectFieldDTO);
    }
}
