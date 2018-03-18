using HouseRent.BaseService.Domain;
using HouseRent.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HouseRent.DataAccess.Repository
{
    public class Repository<T> where T : BaseModel
    {
        //Private fields for the context and the data table
        private RentHouseDbContext context;
        private DbSet<T> dbSet;

        public Repository(RentHouseDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public Repository() : this(new RentHouseDbContext()) { }

        //Returns all the objects from the db for this data table
        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        //Returns all the objects from the db for this data table without tracking them(used for read only) 
        public List<T> GetAllNoTracking(Expression<Func<T, bool>> filter = null)
        {
            return dbSet.Where(filter).AsNoTracking().ToList();
        }

        //Returns all the objects from the db for this data table asynchronously
        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        //Returns all the objects from the db for this data table using the specified filter(lambda expression)
        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return dbSet.Where(filter).ToList();
        }

        //Returns all the objects from the db for this data table using the specified filter(lambda expression) asynchronously
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.Where(filter).ToListAsync();
        }

        //Gets the object from the specified table using its id
        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        //Gets the object from the specified table using a filter(lambda expression) asynchronously
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        //The entity is tracked by the context but does not exists in the db yet
        public void Insert(T model)
        {
            context.Entry(model).State = EntityState.Added;
        }

        //The entity is tracked by the context and exists in the db and some or all of its props are changed
        public void Update(T model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        //The entity is tracked by the context and exists in the db and is marked for deletion
        public void Delete(T model)
        {
            context.Entry(model).State = EntityState.Deleted;
        }
    }
}
