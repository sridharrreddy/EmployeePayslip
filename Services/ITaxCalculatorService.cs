using Services.Enums;

namespace Services
{
    public interface ITaxCalculatorService
    {
        int CalculateIncomeTax(int annualSalary, CalculationFrequency calculationFrequency);
    }
}
