using InventoryManager.Domain.Entities;

namespace InventoryManager.BLL.Hubs.Interface
{
    public interface IInventoryHub
    {
        public Task RecieveMessage(string connectionId, string message);
        public Task RecieveNewComment(string connectionId, Comment comment);
    }
}
