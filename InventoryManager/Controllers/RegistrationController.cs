using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Extension;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AccountDTO accountDTO)
        {
            if (accountDTO == null)
            {
                return BadRequest();
            }
            var resourse = await _registrationService.Authenticate(accountDTO);

            return resourse.ToActionResult();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] AccountDTO accountDTO)
        {
            var resourse = await _registrationService.Registration(accountDTO);

            return resourse.ToActionResult();
        }
    }
}
