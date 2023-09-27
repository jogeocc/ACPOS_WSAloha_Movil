using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Context;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura
{
    public abstract class ServiceBase<T> : IDisposable, IServiceBase<T> where T : class
    {
        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DataContext
        {
            get { return dbContext ?? (dbContext = DatabaseFactory.Get()); }
        }

        public ApplicationDbContext dbContext;
        private bool disposedValue;
        public readonly IDbSet<T> dbset;

        protected ServiceBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
            dbset.AsNoTracking();
            dbContext.Configuration.LazyLoadingEnabled = true;
        }

        protected ServiceBase(IDatabaseFactory databaseFactory, bool noTracking, bool lazyloading = true)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
            if (noTracking)
            {
                dbset.AsNoTracking<T>();
            }
            dbContext.Configuration.LazyLoadingEnabled = lazyloading;
        }
        #region IServiceBase
        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.AsNoTracking().Where(where).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return dbset.AsNoTracking();
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.AsNoTracking().Where(where);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> where, string order, bool descending, int take, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public int Update(T entity)
        {
            try
            {
                dbset.Attach(entity);
                DataContext.Entry(entity).State = EntityState.Modified;

                int result = Save();

                DataContext.Entry(entity).State = EntityState.Detached;
                return result;
            }
            catch
            {
                DataContext.Entry(entity).State = EntityState.Detached;
                throw;
            }
        }

        public int Save()
        {
            return DataContext.SaveChanges();
        }

        public int Create(T entity)
        {
            try
            {
                dbset.Add(entity);
                int result = Save();
                DataContext.Entry(entity).State = EntityState.Detached;
                return result;
            }
            catch
            {
                DataContext.Entry(entity).State = EntityState.Detached;
                throw;
            }
        }

        public int Delete(int id)
        {
            var entity = Get(id);
            dbset.Remove(entity);
            return Save();
        }

        public int Count()
        {
            return dbset.Count();
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return dbset.Count(expression);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
        #endregion
    }
}
