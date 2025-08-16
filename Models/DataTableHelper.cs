using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace AppointmentPlanner.Models
{
    public static class DataTableHelper
    {
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                    if (prop != null && row[column] != DBNull.Value)
                    {
                        prop.SetValue(item, Convert.ChangeType(row[column], prop.PropertyType), null);
                    }
                }
                list.Add(item);
            }
            return list;
        }
    }
}
