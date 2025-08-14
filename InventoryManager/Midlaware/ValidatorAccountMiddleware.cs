using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using System.Net;

namespace InventoryManager.Midlaware
{
    public class ValidatorAccountMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidatorAccountMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IService<Account> accountService)
        {
            var id = httpContext.User.Claims.SingleOrDefault(x => x.Type == CustomClaimType.AccountId)?.Value;
            if (id != null)
            {
                if ((await accountService.GetEntityAsync(x => x.Id == Guid.Parse(id))).Data.Status != AccountStatus.Active)
                {
                    await HandleNotActiveAccountAsync(httpContext);
                    return;
                }
            }
            await _next(httpContext);
        }

        private async Task HandleNotActiveAccountAsync(HttpContext context)
        {
            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Locked;

            ErrorDTO errorDto = new()
            {
                Message = "Bloked",
                StatusCode = (int)HttpStatusCode.Locked
            };

            await response.WriteAsJsonAsync(errorDto);
        }
    }
}
