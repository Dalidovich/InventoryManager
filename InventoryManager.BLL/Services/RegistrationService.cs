using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Extensions;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IService<Account> _accountService;
        private readonly ITokenService _tokenService;

        public RegistrationService(IService<Account> accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<AuthDTO>> Registration(AccountDTO DTO)
        {
            var ClientOnRegistration = (await _accountService.GetEntityAsync(x => x.Login == DTO.Login)).Data;
            if (ClientOnRegistration != null)
            {
                return new StandardResponse<AuthDTO>()
                {
                    Message = "Client with that Email alredy exist",
                    InnerStatusCode = InnerStatusCode.EntityExist,
                };
            }

            _tokenService.CreatePasswordHash(DTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newClients = DTO.CreateEntity(Convert.ToBase64String(passwordSalt), Convert.ToBase64String(passwordHash));
            newClients = (await _accountService.CreateEntityAsync(newClients)).Data;

            return new StandardResponse<AuthDTO>()
            {
                Data = (await Authenticate(DTO)).Data,
                InnerStatusCode = InnerStatusCode.EntityCreate,
            };
        }

        public async Task<BaseResponse<AuthDTO>> Authenticate(AccountDTO DTO)
        {
            var clientResponse = await _accountService.GetEntityAsync(x => x.Login == DTO.Login);
            if (clientResponse.Data == null ||
                !_tokenService.VerifyPasswordHash(DTO.Password, Convert.FromBase64String(clientResponse.Data.Password), Convert.FromBase64String(clientResponse.Data.Salt)))
            {
                return new StandardResponse<AuthDTO>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            var AuthDTO = (await _tokenService.UpdateRefreshToken(clientResponse.Data)).Data;

            return new StandardResponse<AuthDTO>()
            {
                Data = AuthDTO,
                InnerStatusCode = InnerStatusCode.AccountAuthenticate,
            };
        }
    }
}
