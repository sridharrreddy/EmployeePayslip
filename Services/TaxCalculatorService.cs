using Services.ConfigSections;
using Services.Enums;
using System;
using System.Configuration;
using Services.ExtensionMethods;

namespace Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        public int CalculateIncomeTax(int annualSalary, CalculationFrequency calculationFrequency)
        {
            TaxBracketConfigSection settings = TaxBracketConfigSection.LoadSettings();
            var taxInfo = settings.TaxBrackets
                            .Find(taxBracket => (taxBracket.MinSalary == 0 || annualSalary > taxBracket.MinSalary)
                                && (taxBracket.MaxSalary == -1 || annualSalary <= taxBracket.MaxSalary));

            int taxableDollars = annualSalary - taxInfo.MinSalary;
            int baseTax = taxInfo.BaseTax;
            float centsPerDollar = taxInfo.TaxInCentsPerDollar;

            var tax = (baseTax + (taxableDollars * (centsPerDollar / 100))) / (int)calculationFrequency;
            return ((decimal)tax).RoundToNearestInt();
        }
    }
}
