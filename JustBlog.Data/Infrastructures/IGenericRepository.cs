using JustBlog.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Data.Infrastructures
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntityBase<TKey>
        where TKey : struct
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null, bool isDeleted = false);

        TEntity GetById(params object[] keyValues);

        Task<TEntity> GetByIdAsync(params object[] keyValues);

        Task<IEnumerable<TEntity>> GetAllAsync(bool isDeleted = false);

        void Add(TEntity entity);

        Task AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Delete(bool isDeleted = false, params object[] keyValues);

        void Delete(TEntity entity, bool isDeleted = false);

        void DeleteByCondition(Func<TEntity, bool> condition, bool isDeleted = false);

        Task DeleteAsync(TEntity entity, bool isDeleted = false);

        Task DeleteAsync(bool isDeleted = false, params object[] keyValues);

        Task DeleteByConditionAsync(Func<TEntity, bool> condition, bool isDeleted = false);

        IEnumerable<TEntity> Find(Func<TEntity, bool> condition, bool isDeleted = false);

        Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> condition, bool isDeleted = false);

        int Count(bool isDeleted = false);

        Task<int> CountAsync(bool isDeleted = false);
    }
}