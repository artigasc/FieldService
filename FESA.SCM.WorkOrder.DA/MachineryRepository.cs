using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FESA.SCM.Common;
using FESA.SCM.WorkOrder.BE.MachineryBE;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.WorkOrder.DA
{
    public class MachineryRepository : IMachineryRepository
    {
        public IList<Machinery> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IList<Machinery> GetPaginated(Machinery filters, int pageIndex, int pageSize, out int totalRows)
        {
            throw new System.NotImplementedException();
        }

        public Machinery GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(Machinery entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("INSERT_MACHINERY_SP", entity.Id, entity.Brand, entity.Model, entity.SerialNumber,
                    entity.LifeHours, entity.CreatedBy, entity.CreationDate);
        }

        public void Update(Machinery entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("UPDATE_MACHINERY_SP", entity.Id, entity.Brand, entity.Model, entity.SerialNumber,
                    entity.LifeHours, entity.CreatedBy, entity.CreationDate);
        }

        public void Delete(string id, string modifiedBy, DateTime lastModification)
        {
            throw new System.NotImplementedException();
        }
    }
}