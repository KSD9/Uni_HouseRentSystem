using HouseRent.BaseService.Domain;
using HouseRent.DataAccess.Repository;
using HouseRent.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseRent.DataAccess.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        //Creating a private fild that conatins the context
        private RentHouseDbContext context = new RentHouseDbContext();
        Dictionary<Type, object> dictionary = new Dictionary<Type, object>();

        //Generic method for getting the right repository from the context
        public Repository<T> GetRepo<T>() where T : BaseModel
        {
            if (!dictionary.ContainsKey(typeof(T)))
            {
                dictionary.Add(typeof(T), new Repository<T>(context));
            }
            return (Repository<T>)dictionary[typeof(T)];
        }

        //Disposing the context
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        //Applying all the changes and saving them in the db
        public void Save()
        {
            context.SaveChanges();
        }

        //Applying all the changes and saving them in the db without validating it
        public void SaveWithoutValidation()
        {
            context.Configuration.ValidateOnSaveEnabled = false;
            context.SaveChanges();
        }

        //Applying all the changes and saving them in the db asynchronously
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
        }
    }
}
