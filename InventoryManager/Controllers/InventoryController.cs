using InventoryManager.AuthPolicy;
using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Extensions;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Enums;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IAccessAccountToInventoryService _accessAccountToInventoryService;

        public InventoryController(IInventoryService inventoryService, IAccessAccountToInventoryService accessAccountToInventoryService)
        {
            _inventoryService = inventoryService;
            _accessAccountToInventoryService = accessAccountToInventoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid inventoryId)
        {
            var resourse = await _inventoryService.ReadEntityAsync(x => x.Id == inventoryId);

            return resourse.ToActionResult();
        }

        [HttpGet("own")]
        public async Task<IActionResult> GetMyInventory([FromQuery] Guid accountId)
        {
            var resourse = await _inventoryService.ReadEntitiesAsync(x => x.CreatorId == accountId);

            return resourse.ToActionResult();
        }

        [HttpGet("editable")]
        public async Task<IActionResult> GetEditableInventoryFor([FromQuery] Guid accountId)
        {
            var resourse = await _accessAccountToInventoryService.ReadEntitiesAsync(x => x.Id == accountId);

            return resourse.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPost]
        public async Task<IActionResult> Create(InventoryDTO inventoryDTO)
        {
            var resourse = await _inventoryService.CreateEntityAsync(inventoryDTO.CreateEntity());

            return resourse.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{inventoryId}")]
        public async Task<IActionResult> UpdateNewState([FromRoute] Guid inventoryId, [FromQuery] InventoryState newInventoryState)
        {
            var accountId = Guid.Parse(HttpContext.User.FindFirst(CustomClaimType.AccountId)?.Value);
            var resourse = await _inventoryService.UpdateInventoryState(inventoryId, accountId, newInventoryState);

            return resourse.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{inventoryId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid inventoryId)
        {
            var accountId = Guid.Parse(HttpContext.User.FindFirst(CustomClaimType.AccountId)?.Value);
            var resourse = await _inventoryService.DeleteInventory(inventoryId, accountId);

            return resourse.ToActionResult();
        }
    }
}
