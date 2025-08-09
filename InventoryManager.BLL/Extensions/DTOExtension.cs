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

        public static Inventory CreateEntity(this InventoryDTO dto)
        {
            return new Inventory()
            {
                AttachedEntityId = dto.CategoryId,
                CreatorId = dto.CategoryId,
                State = InventoryState.@private,
                Description = dto.Description,
                ImgURL = dto.ImgURL,
                Title = dto.Title,
            };
        }
    }
}
