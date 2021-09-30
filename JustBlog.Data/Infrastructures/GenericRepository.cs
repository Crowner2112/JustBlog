using JustBlog.Models.BaseEntity;
using JustBlog.Models.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Data.Infrastructures
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
         where TEntity : class, IEntityBase<TKey>
         where TKey : struct
    {
        protected readonly JustBlogDbContext Context;

        protected DbSet<TEntity> DbSet;

        protected GenericRepository(JustBlogDbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Add an entity into DbContext, this mean to change the state of entity to added.
        /// Call SaveChanges() method to save data into the database
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        /// <summary>
        /// Add an entity into DbContext, this mean to change the state of entity to added.
        /// Call SaveChangesAsync() method to save data into the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        /// <summary>
        /// Add a list of entities into DbContext, this mean to change state of entities to added.
        /// Call SaveChanges() method to save data into the database
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            this.DbSet.AddRange(entities);
        }

        /// <summary>
        /// Add a list of entities into DbContext, this mean to change state of entities to added.
        /// Call SaveChangesAsync() method to save data into the database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await this.DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// If isDeleted equal false retun number of active records,
        /// otherwise return all record(include active and deleted).
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public virtual int Count(bool isDeleted = false)
        {
            if (!isDeleted) return this.DbSet.Count(x => x.IsDeleted == !DeleteStatus.IsDeleted);
            return this.DbSet.Count();
        }

        public virtual async Task<int> CountAsync(bool isDeleted = false)
        {
            if (!isDeleted)
                return await this.DbSet.CountAsync(x => x.IsDeleted == !DeleteStatus.IsDeleted);
            return await this.DbSet.CountAsync();
        }

        /// <summary>
        /// If isDeleted=true, will delete entity from the database,
        /// otherwise set isDeleted Column of entity equal true.
        /// Call SaveChanges() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual void Delete(bool isDeleted = false, params object[] keyValues)
        {
            var entityExisting = this.DbSet.Find(keyValues);
            if (entityExisting != null)
            {
                if (!isDeleted)
                {
                    entityExisting.IsDeleted = DeleteStatus.IsDeleted;
                    this.Context.Entry(entityExisting).State = EntityState.Modified;
                    return;
                }
                this.DbSet.Remove(entityExisting);
            }
            else
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TEntity)}");
        }

        /// <summary>
        /// If isDeleted=true, will delete entity from the database,
        /// otherwise set isDeleted Column of entity equal true.
        /// Call SaveChanges() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual void Delete(TEntity entity, bool isDeleted = false)
        {
            var entityExisting = this.DbSet.FirstOrDefault(x => x.Id.Equals(entity.Id));
            if (entityExisting != null)
            {
                if (!isDeleted)
                {
                    entityExisting.IsDeleted = DeleteStatus.IsDeleted;
                    this.Context.Entry(entityExisting).State = EntityState.Modified;
                    return;
                }
                this.DbSet.Remove(entityExisting);
            }
            throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TEntity)}");
        }

        /// <summary>
        /// If isDeleted=true, will delete entity from the database,
        /// otherwise set isDeleted Column of entity equal true.
        /// Call SaveChangesAsync() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual async Task DeleteAsync(TEntity entity, bool isDeleted = false)
        {
            var entityExisting = await this.DbSet.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));
            if (entityExisting != null)
            {
                if (!isDeleted)
                {
                    entityExisting.IsDeleted = DeleteStatus.IsDeleted;
                    this.Context.Entry(entityExisting).State = EntityState.Modified;
                    return;
                }
                this.DbSet.Remove(entityExisting);
            }
            throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TEntity)}");
        }

        /// <summary>
        /// If isDeleted=true, will delete entity from the database,
        /// otherwise set isDeleted Column of entity equal true.
        /// Call SaveChangesAsync() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual async Task DeleteAsync(bool isDeleted = false, params object[] keyValues)
        {
            var entityExisting = await this.DbSet.FindAsync(keyValues);
            if (entityExisting != null)
            {
                if (!isDeleted)
                {
                    entityExisting.IsDeleted = DeleteStatus.IsDeleted;
                    this.Context.Entry(entityExisting).State = EntityState.Modified;
                    return;
                }
                this.DbSet.Remove(entityExisting);
            }
            throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TEntity)}");
        }

        /// <summary>
        /// If isDeleted=true, will delete entity match in condition from the database,
        /// otherwise set isDeleted Column of entity match in condition equal true.
        /// Call SaveChanges() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual void DeleteByCondition(Func<TEntity, bool> condition, bool isDeleted = false)
        {
            var query = this.DbSet.Where(condition);
            foreach (var entity in query)
            {
                this.Delete(entity, isDeleted);
            }
        }

        /// <summary>
        /// If isDeleted=true, will delete entity match in condition from the database,
        /// otherwise set isDeleted Column of entity match in condition equal true.
        /// Call SaveChangesAsync() method to save data into the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        public virtual async Task DeleteByConditionAsync(Func<TEntity, bool> condition, bool isDeleted = false)
        {
            var query = this.DbSet.Where(condition);
            foreach (var entity in query)
            {
                await this.DeleteAsync(entity, isDeleted);
            }
        }

        /// <summary>
        /// If isDeleted false then return all return records have column IsDeleted=false(it mean record is active in the database),
        /// otherwise return all records in the database(include active and deleted).
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Func<TEntity, bool> condition, bool isDeleted = false)
        {
            var query = this.DbSet.Where(condition);
            if (!isDeleted)
            {
                return query.Where(x => x.IsDeleted == !DeleteStatus.IsDeleted);
            }
            return query;
        }

        public Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> condition, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If isDeleted equal false return records have IsDeleted Column is flase(It mean records in active),
        /// otherwise return all records(include active and deleted)
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll(Func<TEntity, bool> filter = null, bool isDeleted = false)
        {
            var query = this.DbSet.AsEnumerable();
            if (filter != null)
                query = query.Where(filter);
            if (!isDeleted)
                query = query.Where(x => x.IsDeleted != DeleteStatus.IsDeleted);
            return query;
        }

        /// <summary>
        /// If isDeleted equal false return records have IsDeleted Column is flase(It mean records in active),
        /// otherwise return all records(include active and deleted)
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool isDeleted = false)
        {
            if (!isDeleted)
            {
                return await this.DbSet.Where(x => x.IsDeleted != DeleteStatus.IsDeleted).ToListAsync();
            }
            return await this.DbSet.ToListAsync();
        }

        /// <summary>
        /// Return an entity has primary key match in keyValues paramter(s).
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public TEntity GetById(params object[] keyValues)
        {
            return this.DbSet.Find(keyValues);
        }

        /// <summary>
        /// Return an entity has primary key match in keyValues paramter(s).
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return await this.DbSet.FindAsync(keyValues);
        }

        /// <summary>
        /// Change state of entity to Modified.
        /// Call SaveChanges() method to update data of entity in the database
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public void Update(TEntity entity)
        {
            this.DbSet.Update(entity);
        }
    }
}