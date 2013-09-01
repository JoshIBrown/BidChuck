using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BCWeb.Models
{
    // T has to be a database entity
    public interface IGenericServiceLayer<T> where T : class
    {
        Dictionary<string, string> ValidationDic { get; }
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Delete(int id);
        IEnumerable<T> GetEnumerable();
        IEnumerable<T> GetEnumerable(Expression<Func<T, bool>> predicate);
        T Get(int id);
        bool Exists(int id);
    }
}
