using InventoryManager.Domain.Enums;

namespace InventoryManager.BLL.DTO
{
    public class AccountView
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public AccountStatus Status { get; set; }
        public AccountRole Role { get; set; }
    }
}
