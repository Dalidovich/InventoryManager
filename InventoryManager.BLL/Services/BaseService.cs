using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;
using InventoryManager.Extension;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace InventoryManager.BLL.Services
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        public readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<TEntity>> CreateEntityAsync(TEntity entity)
        {
            var createEntity = await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            var response = new StandardResponse<TEntity>()
            {
                Data = createEntity,
                InnerStatusCode = InnerStatusCode.EntityCreate
            };

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await _repository
                .GetOneWhereAsync(expression);

            if (entity == null)
            {
                return new StandardResponse<bool>()
                {
                    Data = false,
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            var response = new StandardResponse<bool>()
            {
                Data = await _repository.DeleteAsync(expression),
                InnerStatusCode = InnerStatusCode.EntityDelete,
            };

            return response;

        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> GetEntitiesAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await _repository.GetAllWhereAsync(expression);

            var response = new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };

            return response;
        }

        public async Task<BaseResponse<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await _repository.GetOneWhereAsync(expression);
            if (entity == null)
            {
                return new StandardResponse<TEntity>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            var response = new StandardResponse<TEntity>()
            {
                Data = entity,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };

            return response;
        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> ReadEntitiesAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await _repository.ReadAllWhereAsync(expression);

            var response = new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };

            return response;
        }

        public async Task<BaseResponse<TEntity>> ReadEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await _repository.ReadOneWhereAsync(expression);
            if (entity == null)
            {
                return new StandardResponse<TEntity>()
                {
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            var response = new StandardResponse<TEntity>()
            {
                Data = entity,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };

            return response;
        }

        public async Task<BaseResponse<int>> UpdateEntityAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperty)
        {
            var entities = await _repository.ReadOneWhereAsync(whereExpression);
            if (entities == null)
            {
                return new StandardResponse<int>()
                {
                    Data = 0,
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            Expression<Func<TEntity, bool>> checkPositiveBlock = a => a.Timestamp == entities.Timestamp;
            Expression<Func<TEntity, bool>> combined = whereExpression.And(checkPositiveBlock);

            var updatedRow = await _repository.UpdateAsync(combined, setProperty);

            var response = new StandardResponse<int>()
            {
                Data = updatedRow,
                InnerStatusCode = updatedRow != 0 ? InnerStatusCode.EntityUpdate : InnerStatusCode.Conflict,
            };

            return response;
        }
    }
}