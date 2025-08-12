using InventoryManager.BLL.Interfaces;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObjectFieldController : ControllerBase
    {
        private readonly IObjectFieldService _objectFieldService;

        public ObjectFieldController(IObjectFieldService objectFieldService)
        {
            _objectFieldService = objectFieldService;
        }

        //[HttpPost]
        //public async Task<IActionResult>

        [HttpGet("{objectId}")]
        public async Task<IActionResult> Get([FromRoute] Guid objectId)
        {
            var resourse = await _objectFieldService.GetEntitiesAsync(x => x.AttachedEntityId == objectId);

            return resourse.ToActionResult();
        }
    }
}
