using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExtensionMethods
{
    public static class DataTableExtensionMethods
    {
        public static string ToCsv(this DataTable dataTable)
        {
            if(dataTable == null || dataTable.Columns.Count == 0)
            {
                return null;
            }

            StringBuilder stringBuilder = new StringBuilder();
            //Create csv header
            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            stringBuilder.AppendLine(string.Join(",", columnNames));
            //Csv body
            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                stringBuilder.AppendLine(string.Join(",", fields));
            }

            return stringBuilder.ToString();
        }
    }
}
