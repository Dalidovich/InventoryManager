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
        private readonly ITokenService _tokenService;

        public RegistrationController(IRegistrationService registrationService, ITokenService tokenService)
        {
            _registrationService = registrationService;
            _tokenService = tokenService;
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromQuery] Guid accountId, [FromQuery] string refreshToken)
        {
            var resourse = await _tokenService.RefreshJWTToken(accountId, refreshToken);

            return resourse.ToActionResult();
        }
    }
}
