using InventoryManager.AuthPolicy;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;
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

        [Authorize(Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpGet("search/{searchField}")]
        public async Task<IActionResult> Get([FromRoute] string searchField, [FromQuery] string? searchParametr)
        {
            BaseResponse<IEnumerable<Account>> resourse;
            switch (searchParametr)
            {
                case "login":
                    resourse = await _accountService.ReadOrderedEntitiesAsync(x => x.Login.StartsWith(searchParametr), x => x.Login);
                    return resourse.ToActionResult();
                case "email":
                    resourse = await _accountService.ReadOrderedEntitiesAsync(x => x.Email.StartsWith(searchParametr), x => x.Email);
                    return resourse.ToActionResult();
                default:
                    resourse = await _accountService.ReadOrderedEntitiesAsync(x => x.Id != null, x => x.Login);
                    return resourse.ToActionResult();
            }
        }

        [Authorize(Roles = "1", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var resourse = await _accountService.DeleteEntityAsync(x => x.Id == id);

            return resourse.ToActionResult();
        }

        [Authorize(Roles = "1", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromQuery] AccountRole newRole, [FromQuery] DateTime timestamp)
        {
            var resourse = await _accountService.UpdateEntityAsync(x => x.Id == id, x => x.SetProperty(x => x.Role, newRole), timestamp);

            return resourse.ToActionResult();
        }

        [Authorize(Roles = "1", Policy = AuthPolicyName.ActiveStatusPolicyRequire)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromQuery] AccountStatus newStatus, [FromQuery] DateTime timestamp)
        {
            var resourse = await _accountService.UpdateEntityAsync(x => x.Id == id, x => x.SetProperty(x => x.Status, newStatus), timestamp);

            return resourse.ToActionResult();
        }
    }
}
