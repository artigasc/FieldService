using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using FESA.SCM.Web.Models;

namespace FESA.SCM.Web.Helpers
{

    public static class ConvertToDataTable
    {
        public static DataTable ToDataTable<Assignments>(List<Assignments> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Assignments));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (Assignments item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    var valor = props[i].GetValue(item);
                    values[i] = valor;
                }
                table.Rows.Add(values);
            }
            return table;

        }
    }
}