using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.Repository.Abstract
{
    public interface IRepository<Model>
    {
        IEnumerable<Model> SelectAll();
        Model SelectById(int RecordId);
        IQueryable<Model> SelectByProperty(Expression<Func<Model, bool>> predicate);
        bool Insert(Model NewModel);
        Task<bool> InsertAsync(Model NewModel);
        bool Update(Model ObjectModel);
        Task<bool> UpdateAsync(Model ObjectModel);
        bool Delete(int Id);
        Task<bool> DeleteAsync(int Id);
    }
}