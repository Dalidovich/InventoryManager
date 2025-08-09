using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;
using Microsoft.AspNetCore.Mvc;
namespace InventoryManager.Extension
{
    public static class CastinnerResponseExtension
    {
        public static IActionResult ToActionResult<T>(this BaseResponse<T> resource)
        {
            switch (int.Parse(((int)resource.InnerStatusCode).ToString()[0].ToString()))
            {
                case 0:
                    return new NotFoundResult();
                case 1:
                    return new CreatedResult("", resource);
                case 2:
                    return new OkObjectResult(resource);
                case 3:
                    return new NoContentResult();
                case 4:
                    return new OkObjectResult(resource);
                case 5:
                    return new OkObjectResult(resource);
                case 6:
                    return new AcceptedResult("", resource);
                default:
                    switch (resource.InnerStatusCode)
                    {
                        case InnerStatusCode.Unauthorized:
                            return new UnauthorizedResult();
                        case InnerStatusCode.OK:
                            return new OkObjectResult(resource);
                        case InnerStatusCode.OKNoContent:
                            return new NoContentResult();
                        case InnerStatusCode.Forbiden:
                            return new ForbidResult();
                        case InnerStatusCode.Conflict:
                            return new ConflictResult();
                        case InnerStatusCode.UnsupportedMediaType:
                            return new UnsupportedMediaTypeResult();
                        case InnerStatusCode.Locked:
                            return new StatusCodeResult((int)InnerStatusCode.Locked);
                        default:
                            return new StatusCodeResult(500);
                    }
            }
        }
    }
}
