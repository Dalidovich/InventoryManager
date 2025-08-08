using InventoryManager.BLL.Hubs.Interface;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace InventoryManager.BLL.Hubs
{
    public class InventoryHub : Hub<IInventoryHub>
    {
        private readonly IService<Comment> _commentService;
        private readonly IService<Account> _accountService;

        public InventoryHub(IService<Comment> commentService, IService<Account> accountService)
        {
            _commentService = commentService;
            _accountService = accountService;
        }

        public async Task JoinToInventory(Guid inventoryId, Guid accountId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, inventoryId.ToString());
            await Clients.Group(inventoryId.ToString()).RecieveMessage(Context.ConnectionId, $"{accountId} connected to inventory {inventoryId}");
        }

        public async Task RemoveFromPresentation(Guid inventoryId, Guid accountId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, inventoryId.ToString());
            await Clients.Group(inventoryId.ToString()).RecieveMessage(Context.ConnectionId, $"{accountId} remove on inventory {inventoryId}");
        }

        public async Task AddComment(Guid inventoryId, Guid accountId, string content)
        {

            var account = await _accountService.ReadEntityAsync(x => x.Id == accountId);
            var comment = await _commentService.CreateEntityAsync(new Comment()
            {
                Content = content,
                CreatorId = accountId,
                AttachedEntityId = inventoryId,
                Author = account.Data
            });
            if (comment.InnerStatusCode == InnerStatusCode.EntityCreate)
            {
                await Clients.Group(inventoryId.ToString()).RecieveNewComment(Context.ConnectionId, comment.Data);
            }
        }
    }
}
