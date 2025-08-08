using InventoryManager.Domain.Enums;

namespace InventoryManager.Domain.InnerResponse
{
    public abstract class BaseResponse<T>
    {
        public virtual T Data { get; set; }
        public virtual InnerStatusCode InnerStatusCode { get; set; }
        public virtual string Message { get; set; }
    }
}
