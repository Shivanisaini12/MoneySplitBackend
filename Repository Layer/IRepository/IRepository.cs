using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int Id);
        Task<T> GetT(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetByExpression(Expression<Func<T, bool>> predicate);
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
