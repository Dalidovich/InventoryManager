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
    public class AccountController : ControllerBase
    {
        private readonly IService<Account> _accountService;

        public AccountController(IService<Account> accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resourse = await _accountService.ReadEntitiesAsync(x => x.Id != null);

            return resourse.ToActionResult();
        }

        [Authorize(Roles = "Admin", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var resourse = await _accountService.DeleteEntityAsync(x => x.Id == id);

            return resourse.ToActionResult();
        }

        [Authorize(Roles = "Admin", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromQuery] AccountRole newRole)
        {
            var resourse = await _accountService.UpdateEntityAsync(x => x.Id == id, x => x.SetProperty(x => x.Role, newRole));

            return resourse.ToActionResult();
        }

        [Authorize(Roles = "Admin", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromQuery] AccountStatus newStatus)
        {
            var resourse = await _accountService.UpdateEntityAsync(x => x.Id == id, x => x.SetProperty(x => x.Status, newStatus));

            return resourse.ToActionResult();
        }
    }
}
