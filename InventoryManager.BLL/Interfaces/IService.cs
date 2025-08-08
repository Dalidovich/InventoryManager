using InventoryManager.Domain.InnerResponse;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace InventoryManager.BLL.Interfaces
{
    public interface IService<TEntity>
    {
        public Task<BaseResponse<TEntity>> CreateEntityAsync(TEntity entity);
        public Task<BaseResponse<bool>> DeleteEntityAsync(Expression<Func<TEntity, bool>> expression);
        public Task<BaseResponse<int>> UpdateEntityAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperty);
        public Task<BaseResponse<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> expression);
        public Task<BaseResponse<IEnumerable<TEntity>>> GetEntitiesAsync(Expression<Func<TEntity, bool>> expression);
        public Task<BaseResponse<TEntity>> ReadEntityAsync(Expression<Func<TEntity, bool>> expression);
        public Task<BaseResponse<IEnumerable<TEntity>>> ReadEntitiesAsync(Expression<Func<TEntity, bool>> expression);
    }
}
