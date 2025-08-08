using InventoryManager.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace InventoryManager.DAL.Repositories.Implements
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _db;
        private readonly DbSet<T> _table;

        public BaseRepository(AppDBContext db)
        {
            _db = db;
            _table = _db.GetTable<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            var createdEntity = await _table.AddAsync(entity);

            return createdEntity.Entity;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            await _table.Where(expression).ExecuteDeleteAsync();

            return true;
        }

        public T Update(T entity)
        {
            var updatedEntity = _table.Update(entity);

            return updatedEntity.Entity;
        }

        public async Task<int> UpdateAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperty)
        {
            return await _table.Where(whereExpression).ExecuteUpdateAsync(setProperty);
        }

        public async Task<IEnumerable<T>> GetAllWhereOrderByAsync<T1>(Expression<Func<T, bool>> expression, Expression<Func<T, T1>> orderExpression)
        {
            return await _table.Where(expression).OrderBy(orderExpression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> ReadAllWhereOrderByAsync<T1>(Expression<Func<T, bool>> expression, Expression<Func<T, T1>> orderExpression)
        {
            return await _table.AsNoTracking().Where(expression).OrderBy(orderExpression).ToListAsync();
        }

        public async Task<IEnumerable<T>> ReadAllWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<T?> GetOneWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.Where(expression).SingleOrDefaultAsync();
        }

        public async Task<T?> ReadOneWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.AsNoTracking().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            await _db.SaveChangesAsync();

            return true;
        }
    }
}