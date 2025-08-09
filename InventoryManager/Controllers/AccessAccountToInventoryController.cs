using InventoryManager.AuthPolicy;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessAccountToInventoryController : ControllerBase
    {
        private readonly IAccessAccountToInventoryService _accessAccountToInventoryService;

        public AccessAccountToInventoryController(IAccessAccountToInventoryService accessAccountToInventoryService, IService<Inventory> inventoryService)
        {
            _accessAccountToInventoryService = accessAccountToInventoryService;
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccessAccountToInventory accessAccountToInventory)
        {
            var resource = await _accessAccountToInventoryService.CreateAccessAccountToInventory(accessAccountToInventory);

            return resource.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var accountId = Guid.Parse(HttpContext.User.FindFirst(CustomClaimType.AccountId)?.Value);
            var resource = await _accessAccountToInventoryService.DeleteAccessAccountToInventory(id, accountId);

            return resource.ToActionResult();
        }
    }
}
