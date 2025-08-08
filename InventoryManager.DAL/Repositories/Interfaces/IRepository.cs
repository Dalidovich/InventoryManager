using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace InventoryManager.DAL.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public Task<T> AddAsync(T entity);
        public T Update(T entity);
        public Task<int> UpdateAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperty);
        public Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> ReadAllWhereAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> GetAllWhereOrderByAsync<T1>(Expression<Func<T, bool>> expression, Expression<Func<T, T1>> orderExpression);
        public Task<IEnumerable<T>> ReadAllWhereOrderByAsync<T1>(Expression<Func<T, bool>> expression, Expression<Func<T, T1>> orderExpression);
        public Task<T?> GetOneWhereAsync(Expression<Func<T, bool>> expression);
        public Task<T?> ReadOneWhereAsync(Expression<Func<T, bool>> expression);
        public Task<bool> SaveAsync();
    }
}
