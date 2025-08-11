using InventoryManager.AuthPolicy;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPost("{inventoryObjectId}")]
        public async Task<IActionResult> Create([FromRoute] Guid inventoryObjectId, [FromQuery] Guid accountId)
        {
            var resourse = await _likeService.CreateLike(inventoryObjectId, accountId);

            return resourse.ToActionResult();
        }

        [HttpGet("{inventoryObjectId}")]
        public async Task<IActionResult> Get([FromRoute] Guid inventoryObjectId)
        {
            var resourse = await _likeService.GetEntityAsync(x => x.AttachedEntityId == inventoryObjectId);

            return resourse.ToActionResult();
        }

        [HttpDelete("{inventoryObjectId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid inventoryObjectId, [FromQuery] Guid accountId)
        {
            var resourse = await _likeService.DeleteEntityAsync(x => x.AttachedEntityId == inventoryObjectId && x.CreatorId == accountId);

            return resourse.ToActionResult();
        }
    }
}
