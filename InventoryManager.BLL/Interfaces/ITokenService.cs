using InventoryManager.BLL.DTO;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(Account client);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] passwordSalt);
        public string GetRefreshToken();
        public Task<StandardResponse<AuthDTO>> RefreshJWTToken(Guid accountId, string refreshTokenStr);
        public Task<StandardResponse<AuthDTO>> UpdateRefreshToken(Account account);
    }
}
