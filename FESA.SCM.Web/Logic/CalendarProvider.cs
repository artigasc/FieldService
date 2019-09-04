using System;
using System.Collections.Generic;
using System.Linq;
using FESA.SCM.Web.DataAccess;

namespace FESA.SCM.Web.Logic
{
    public static class CalendarProvider
    {
        public static IList<dynamic> GetItems(int office, int costCenter, DateTime fechaInicio, DateTime fechaFin)
        {
            var fesaDataSource = new FesaDataSourceDataContext();
            var spResult = fesaDataSource.GET_ALL_ACTIVITIES_SP(office, costCenter, fechaInicio, fechaFin).ToList();
            List<dynamic> activities = new List<dynamic>();
            foreach (var item in spResult)
            {
                activities.Add((dynamic) item);
            }
            return activities;
        }
    }
}