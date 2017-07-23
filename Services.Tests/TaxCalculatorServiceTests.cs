using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.ConfigSections;
using Services.Enums;
using System.Linq;

namespace Services.Tests
{
    [TestClass]
    public class TaxCalculatorServiceTests
    {
        ITaxCalculatorService _taxCalculator;
        TaxBracketConfigSection _settings;

        [TestInitialize]
        public void TestInitialize()
        {
            _taxCalculator = new TaxCalculatorService();
            _settings = TaxBracketConfigSection.LoadSettings();
        }

        [TestMethod]
        public void CalculateIncomeTax_ForZeroSalary_ReturnsZero()
        {
            //Arrange

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(0, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(0, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_ForSalaryLessThanOrEqualTo18200_ReturnsZero()
        {
            //Arrange

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(18200, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(0, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_ForSalaryOf18201_ReturnsTaxRoundedToZero()
        {
            //Arrange

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(18201, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(0, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_ForSalaryOf18300_Returns2()
        {
            //Arrange
            //(100 * .19)/12 = 1.58 => RoundUp

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(18300, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(2, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_ForSalaryOfSlab2BorderCase_ReturnsSlab2Tax()
        {
            //Arrange
            var annualSalary = 37000;
            //(0 + 18800 * .19)/12 = 297.666 => RoundUp to 298

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(annualSalary, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(298, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_ForMaxSalarySlab_ReturnsCorrectTax()
        {
            //Arrange
            var annualSalary = 180050;
            //(54547 + 50 * .45)/12 = 4547.458 => RoundUp to 4547

            //Act
            var tax = _taxCalculator.CalculateIncomeTax(annualSalary, CalculationFrequency.Monthly);

            //Assert
            Assert.AreEqual(4547, tax);
        }

        [TestMethod]
        public void CalculateIncomeTax_TestAllSalaryRangesAreInTaxBrackets()
        {
            //Arrange
            var annualSalary = 0;
            var erroredOut = false;

            //Act
            while (annualSalary < _settings.TaxBrackets.OrderByDescending(t=>t.MinSalary).First().MinSalary + 1)
            {
                try
                {
                    _taxCalculator.CalculateIncomeTax(annualSalary, CalculationFrequency.Monthly);
                    annualSalary++;
                }
                catch
                {
                    erroredOut = true;
                }
            }

            //Assert
            Assert.IsFalse(erroredOut);
        }
    }
}
