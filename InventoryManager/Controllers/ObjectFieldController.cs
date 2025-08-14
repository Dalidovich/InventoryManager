using InventoryManager.BLL.DTO;
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

        [HttpPost("master")]
        public async Task<IActionResult> CreateMaster([FromBody] MasterObjectFieldDTO masterObjectFieldDTO)
        {
            var resourse = await _objectFieldService.CreateMasterObjectField(masterObjectFieldDTO);

            return resourse.ToActionResult();
        }

        [HttpPost("content")]
        public async Task<IActionResult> CreateContent([FromBody] ContentObjectFieldDTO contentObjectFieldDTO)
        {
            var resourse = await _objectFieldService.CreateContentObjectField(contentObjectFieldDTO);

            return resourse.ToActionResult();
        }

        [HttpGet("{objectId}")]
        public async Task<IActionResult> Get([FromRoute] Guid objectId)
        {
            var resourse = await _objectFieldService.GetEntitiesAsync(x => x.AttachedEntityId == objectId);

            return resourse.ToActionResult();
        }

        [HttpPut("master")]
        public async Task<IActionResult> UpdateMaster([FromBody] UpdateMasterObjectFieldDTO masterObjectFieldDTO)
        {
            var resourse = await _objectFieldService.UpdateMasterObjectField(masterObjectFieldDTO);

            return resourse.ToActionResult();
        }

        [HttpPut("content")]
        public async Task<IActionResult> UpdateContent([FromBody] UpdateContentObjectFieldDTO updateContentObjectFieldDTO)
        {
            var resourse = await _objectFieldService.UpdateContentObjectField(updateContentObjectFieldDTO);

            return resourse.ToActionResult();
        }
    }
}
