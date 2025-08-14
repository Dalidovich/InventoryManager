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

            return new StandardResponse<TEntity>()
            {
                Data = createEntity,
                InnerStatusCode = InnerStatusCode.EntityCreate
            };
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

            return new StandardResponse<bool>()
            {
                Data = await _repository.DeleteAsync(expression),
                InnerStatusCode = InnerStatusCode.EntityDelete,
            };

        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> GetEntitiesAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await _repository.GetAllWhereAsync(expression);

            return new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
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

            return new StandardResponse<TEntity>()
            {
                Data = entity,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> ReadEntitiesAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await _repository.ReadAllWhereAsync(expression);

            return new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
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

            return new StandardResponse<TEntity>()
            {
                Data = entity,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> ReadOrderedEntitiesAsync<T>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, T>> orderExpression)
        {
            var entities = await _repository.ReadAllWhereOrderByAsync(whereExpression, orderExpression);

            return new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
        }

        public async Task<BaseResponse<IEnumerable<TEntity>>> GetOrderedEntitiesAsync<T>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, T>> orderExpression)
        {
            var entities = await _repository.GetAllWhereOrderByAsync(whereExpression, orderExpression);

            return new StandardResponse<IEnumerable<TEntity>>()
            {
                Data = entities,
                InnerStatusCode = InnerStatusCode.EntityRead,
            };
        }

        public async Task<BaseResponse<int>> UpdateEntityAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperty, DateTime timestamp)
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

            Expression<Func<TEntity, bool>> checkPositiveBlock = a => a.Timestamp == timestamp;
            Expression<Func<TEntity, bool>> combinedWhere = whereExpression.And(checkPositiveBlock);

            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateTimestamp = a => a.SetProperty(x => x.Timestamp, DateTime.UtcNow);
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> combinedSetProperty = setProperty.And(updateTimestamp);

            var updatedRow = await _repository.UpdateAsync(combinedWhere, combinedSetProperty);

            return new StandardResponse<int>()
            {
                Data = updatedRow,
                InnerStatusCode = updatedRow != 0 ? InnerStatusCode.EntityUpdate : InnerStatusCode.Conflict,
            };
        }
    }
}