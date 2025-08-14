using InventoryManager.AuthPolicy;
using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryObjectController : ControllerBase
    {
        private readonly IInventoryObjectService _inventoryObjectService;

        public InventoryObjectController(IInventoryObjectService inventoryObjectService)
        {
            _inventoryObjectService = inventoryObjectService;
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryObjectDTO inventoryObjectDTO)
        {
            var resourse = await _inventoryObjectService.CreateInventoryObject(inventoryObjectDTO);

            return resourse.ToActionResult();
        }

        [HttpGet("{inventoryId}/{isTemplate}")]
        public async Task<IActionResult> Get([FromRoute] Guid inventoryId, [FromRoute] bool isTemplate)
        {
            var resourse = await _inventoryObjectService
                .ReadOrderedEntitiesAsync(x => x.AttachedEntityId == inventoryId && x.IsTemplate == isTemplate,
                x => x.SequenceId);

            return resourse.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpDelete("{inventoryObjectId}")]
        public async Task<IActionResult> DeleteInventoryObject([FromRoute] Guid inventoryObjectId, [FromQuery] Guid accountId)
        {
            var resourse = await _inventoryObjectService.DeleteInventoryObject(inventoryObjectId, accountId);

            return resourse.ToActionResult();
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{inventoryObjectId}")]
        public async Task<IActionResult> UpdateInventoryObjectTitle([FromRoute] Guid inventoryObjectId, [FromQuery] Guid accountId, [FromQuery] string newTitle, [FromQuery] DateTime timestamp)
        {
            var resourse = await _inventoryObjectService.UpdateInventoryObjectTitle(inventoryObjectId, accountId, newTitle, timestamp);

            return resourse.ToActionResult();
        }
    }
}
