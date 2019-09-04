using FESA.SCM.Common;
using FESA.SCM.WorkOrder.BE;
using FESA.SCM.WorkOrder.BE.OrderPerClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WorkOrder.DA
{
    public class OrderPerClientRepository : IOrderPerClientRepository
    {
        public IList<OrderPerClient> GetNumberOrders(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter)
        {
            IList<OrderPerClient> orders;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_NUMBER_ORDERS_BY_CUSTOMERID_SP", supervisorid, dateIni, dateFin, idsOffice, idsCostCenter ))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    orders = new List<OrderPerClient>();
                    while (reader.Read())
                    {
                        orders.Add(new OrderPerClient
                        {
                            OrderId = DataConvert.ToString(reader["ID"]),
                            Quantity = DataConvert.ToInt32(reader["total"]),
                            CompanyName = DataConvert.ToString(reader["NAME"]),
                            AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"])
                        });
                    }
                }
            }
            return orders;
        }
    }
}
