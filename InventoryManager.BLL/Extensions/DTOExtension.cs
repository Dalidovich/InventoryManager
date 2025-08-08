using InventoryManager.BLL.DTO;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;

namespace InventoryManager.BLL.Extensions
{
    public static class DTOExtension
    {
        public static Account CreateEntity(this AccountDTO dto, string salt, string hash)
        {
            return new Account()
            {
                Login = dto.Login,
                Salt = salt,
                Password = hash,
                Email = dto.Email,
                Role = AccountRole.None,
                Status = AccountStatus.Active,
            };
        }
    }
}
