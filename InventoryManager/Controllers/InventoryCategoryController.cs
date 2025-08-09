using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryCategoryController
    {
        private readonly IService<InventoryCategory> _inventoryCategoryService;

        public InventoryCategoryController(IService<InventoryCategory> inventoryCategoryService)
        {
            _inventoryCategoryService = inventoryCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resourse = await _inventoryCategoryService.GetEntitiesAsync(x => x.Id != null);

            return resourse.ToActionResult();
        }
    }
}
