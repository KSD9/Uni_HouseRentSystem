using HouseRent.Common.Interfaces;
using HouseRent.BaseService.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HouseRent.DataAccess.UnitOfWork;

namespace HouseRent.BaseService
{
    public abstract class BaseService<T> where T : BaseModel
    {
        public IValidation ModelStateWrapper { get; set; }
        public UnitOfWork UnitOfWork { get; set; }

        public BaseService(UnitOfWork unitOfWork, IValidation modelStateWrapper)
        {
            ModelStateWrapper = modelStateWrapper;
            UnitOfWork = unitOfWork;
        }

        //Method that is going to be overriden in the classes that inhert BaseService
        public abstract bool Validation(T model);

        //Calling the GetAll() method in the repository
        public virtual List<T> GetAll()
        {
            return UnitOfWork.GetRepo<T>().GetAll();
        }

        //Calling the GetAllNoTracking() method in the repository
        public virtual List<T> GetAllNoTracking(Expression<Func<T, bool>> filter = null)
        {
            return UnitOfWork.GetRepo<T>().GetAllNoTracking(filter);
        }

        //Calling the GetAll() with lambda expression method in the repository
        public virtual List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return UnitOfWork.GetRepo<T>().GetAll(filter);
        }

        //Calling the GetAllAsync() with lambda expression method in the repository
        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return await UnitOfWork.GetRepo<T>().GetAllAsync(filter);
        }

        //Calling the GetAllAsync() method in the repository
        public virtual async Task<List<T>> GetAllAsync()
        {
            return await UnitOfWork.GetRepo<T>().GetAllAsync();
        }

        //Calling the Get() method using the id in the repository
        public virtual T Get(int id)
        {
            return UnitOfWork.GetRepo<T>().Get(id);
        }

        //Calling the GetAsync() method using a lambda expression in the repository
        public async virtual Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
        {
            return await UnitOfWork.GetRepo<T>().GetAsync(filter);
        }

        //Calling the Insert() method in the repository
        public virtual void Insert(T model)
        {
            UnitOfWork.GetRepo<T>().Insert(model);
        }

        //Calling the Update() method in the repository
        public virtual void Update(T model)
        {
            UnitOfWork.GetRepo<T>().Update(model);
        }

        //Calling the Delete() method in the repository
        public virtual void Delete(T model)
        {
            UnitOfWork.GetRepo<T>().Delete(model);
        }

        //Calling the SaveChangesAsync() method in the repository
        public async Task SaveChangesAsync()
        {
            await UnitOfWork.SaveAsync();
        }

        //Calling the SaveChanges() method in the repository
        public void SaveChanges()
        {
            UnitOfWork.Save();
        }
    }
}
