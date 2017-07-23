using Services.Enums;
using Services.ExtensionMethods;
using Services.Models;

namespace Services
{
    public class PayslipService : IPayslipService
    {
        private readonly ITaxCalculatorService _taxCalculator;
        public PayslipService(ITaxCalculatorService taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public PayslipInfo GeneratePayslipForEmployee(EmployeeInformation data)
        {
            var payslip = new PayslipInfo
            {
                Name = string.Concat(data.EmployeeFirstName, " ", data.EmployeeLastName),
                GrossIncome = ((decimal)(data.AnnualSalary / (int)data.PayslipFrequency)).RoundToNearestInt(),
                IncomeTax = _taxCalculator.CalculateIncomeTax(data.AnnualSalary, data.PayslipFrequency),
                PayPeriod = data.PayPeriod
            };
            payslip.Super = ((decimal)(payslip.GrossIncome * data.SuperRate / 100)).RoundToNearestInt();
            return payslip;
        }
    }
}
