using System;
using System.Collections.Generic;

namespace FESA.SCM.Common.Base
{
    public interface IBaseRepository<T> where T: class
    {
        IList<T> GetAll();
        IList<T> GetPaginated(T filters, int pageIndex, int pageSize, out int totalRows);
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(string id, string modifiedBy, DateTime lastModification);
    }
}