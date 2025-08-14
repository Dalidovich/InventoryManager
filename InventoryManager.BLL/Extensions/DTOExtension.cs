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

        public static InventoryObject CreateEntity(this InventoryObjectDTO dto, int nextSequenceId)
        {
            return new InventoryObject()
            {
                AttachedEntityId = dto.InventoryId,
                CreatorId = dto.CreatorId,
                Title = dto.Title,
                IsTemplate = dto.IsTemplate,
                SequenceId = nextSequenceId,
            };
        }

        public static ObjectField CreateEntity(this MasterObjectFieldDTO dto)
        {
            return new ObjectField()
            {
                Title = dto.Title,
                Description = dto.Description,
                Visible = dto.Visible,
                MasterFieldId = null,
                Content = "",
                Type = dto.Type,
                CreatorId = dto.CreatorId,
                AttachedEntityId = dto.InventoryObjectId,
            };
        }

        public static ObjectField CreateEntity(this ContentObjectFieldDTO dto, ObjectField objectField)
        {
            return new ObjectField()
            {
                Title = objectField.Title,
                Description = objectField.Description,
                Visible = objectField.Visible,
                Type = objectField.Type,
                MasterFieldId = dto.MasterFieldId,
                Content = dto.Content,
                CreatorId = dto.CreatorId,
                AttachedEntityId = dto.InventoryObjectId,
            };
        }

        public static ObjectField UpdateData(this UpdateMasterObjectFieldDTO dto, ObjectField objectField)
        {
            return new ObjectField()
            {
                Title = dto.Title ?? objectField.Title,
                Description = dto.Description ?? objectField.Description,
                Visible = dto.Visible ?? objectField.Visible,
                Type = dto.Type ?? objectField.Type,
                MasterFieldId = objectField.MasterFieldId,
                Content = objectField.Content,
                CreatorId = objectField.CreatorId,
                AttachedEntityId = objectField.AttachedEntityId,
                CreatedAt = objectField.CreatedAt,
                Id = objectField.Id,
                Timestamp = objectField.Timestamp,
            };
        }
    }
}
