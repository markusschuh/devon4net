using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OASP4Net.Domain.Repository
{

    public abstract class EntityService<T> : IEntityService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T> _repository;

        protected EntityService(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public void Create(T entity)
        {
            if (entity == null) return;
            _repository.Create(entity);
            _unitOfWork.Commit();
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            return _repository.Get(predicate);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) return;
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }

        public void DeleteById(object id)
        {
            if (id == null) return;
            _repository.DeleteById(id);
            _unitOfWork.Commit();
        }

        public void Delete(Expression<Func<T, bool>> @where)
        {
            _repository.Delete(@where);
            _unitOfWork.Commit();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) return;
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return _repository.GetAll(predicate);
        }

        #region async methods
        public Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            return _repository.GetAllAsync(predicate);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            return _repository.GetAsync(predicate);
        }
        #endregion region
    }
}
