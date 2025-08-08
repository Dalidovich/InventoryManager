using InventoryManager.Domain.Enums;

namespace InventoryManager.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public AccountStatus Status { get; set; }
        public AccountRole Role { get; set; }

        public Account() : base()
        {

        }
    }
}
