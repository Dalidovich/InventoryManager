using InventoryManager.BLL.DTO;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface IRegistrationService
    {
        public Task<BaseResponse<AuthDTO>> Registration(AccountDTO DTO);
        public Task<BaseResponse<AuthDTO>> Authenticate(AccountDTO DTO);
    }
}
