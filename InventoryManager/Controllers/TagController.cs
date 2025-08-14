using InventoryManager.AuthPolicy;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IService<Tag> _tagSrvice;

        public TagController(IService<Tag> tagSrvice)
        {
            _tagSrvice = tagSrvice;
        }

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPost("{objectId}")]
        public async Task<IActionResult> Create([FromRoute] Guid objectId, [FromQuery] string title, [FromQuery] Guid accountId)
        {
            var resourse = await _tagSrvice.CreateEntityAsync(new Tag()
            {
                AttachedEntityId = objectId,
                Title = title,
                CreatorId = accountId
            });

            return resourse.ToActionResult();
        }
    }
}
