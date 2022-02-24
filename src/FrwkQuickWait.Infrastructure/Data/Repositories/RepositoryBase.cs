using FrwkQuickWait.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FrwkQuickWait.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> DbSet;

        protected RepositoryBase(DbContext context)
        {
            this.context = context;
            this.DbSet = context.Set<T>();
        }

        public virtual async Task SaveAsync(T obj)
        {
            await DbSet.AddAsync(obj);
            await context.SaveChangesAsync();
        }


        public virtual void Update(T obj)
        {
           DbSet.Update(obj);
           context.SaveChanges();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Delete(T obj)
        {
            DbSet.Remove(obj);
            context.SaveChanges();
        }

        public virtual void DeleteMany(IEnumerable<T> obj)
        {
            DbSet.RemoveRange(obj);
            context.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
             => DbSet.ToList();

        public void Dispose()
            => context.Dispose();
    }
}
