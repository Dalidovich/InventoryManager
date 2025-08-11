using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;

namespace InventoryManager.BLL.Services
{
    public class LikeService : BaseService<Like>, ILikeService
    {
        private readonly IPrivacyCheckerService _privacyCheckerService;
        private readonly IInventoryObjectService _inventoryObjectService;

        public LikeService(IRepository<Like> repository, IPrivacyCheckerService privacyCheckerService, IInventoryObjectService inventoryObjectService) : base(repository)
        {
            _privacyCheckerService = privacyCheckerService;
            _inventoryObjectService = inventoryObjectService;
        }

        public async Task<BaseResponse<Like>> CreateLike(Guid inventoryObjectId, Guid accountId)
        {
            var inventoryObjectWithoutYouLike = await _inventoryObjectService.ReadEntityAsync(x => x.Id == inventoryObjectId &&
                x.Likes.Where(e => e.CreatorId == accountId).Count() == 0);

            if (inventoryObjectWithoutYouLike.InnerStatusCode == InnerStatusCode.EntityRead)
            {
                var newLike = await CreateEntityAsync(new Like()
                {
                    AttachedEntityId = inventoryObjectId,
                    CreatorId = accountId
                });

                return new StandardResponse<Like>()
                {
                    Data = newLike.Data,
                    InnerStatusCode = InnerStatusCode.EntityCreate
                };
            }

            return new StandardResponse<Like>()
            {
                InnerStatusCode = InnerStatusCode.Locked,
            };
        }
    }
}
