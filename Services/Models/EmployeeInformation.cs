using Services.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services.Models
{
    public class EmployeeInformation
    {
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public CalculationFrequency PayslipFrequency { get; set; }
        public int AnnualSalary { get; set; }
        public float SuperRate { get; set; }
        public string PayPeriod { get; set; }

        public static List<EmployeeInformation> LoadFromDataTable(DataTable dataTable)
        {
            var list = (from row in dataTable.AsEnumerable()
                        select new EmployeeInformation()
                        {
                            EmployeeFirstName = Convert.ToString(row["first name"]),
                            EmployeeLastName = Convert.ToString(row["last name"]),
                            PayslipFrequency = CalculationFrequency.Monthly,
                            AnnualSalary = Convert.ToInt32(row["annual salary"]),
                            SuperRate = float.Parse(Convert.ToString(row["super rate (%)"]).Replace("%", "").Trim()),
                            PayPeriod = Convert.ToString(row["payment start date"]),
                        }).ToList();

            return list;
        }

        public bool IsModelValid()
        {
            //TODO: Move rules to config file. Demonstrated elsewhere.
            if (AnnualSalary <= 0 || SuperRate < 0 || SuperRate > 50 || (string.IsNullOrWhiteSpace(EmployeeFirstName) && string.IsNullOrWhiteSpace(EmployeeLastName)))
            {
                return false;
            }
            return true;
        }
    }
}
