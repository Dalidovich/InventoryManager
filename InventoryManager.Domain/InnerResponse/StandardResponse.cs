using InventoryManager.Domain.Enums;

namespace InventoryManager.Domain.InnerResponse
{
    public class StandardResponse<T> : BaseResponse<T>
    {
        public override string Message { get; set; } = null!;
        public override InnerStatusCode InnerStatusCode { get; set; }
        public override T Data { get; set; }
    }
}
