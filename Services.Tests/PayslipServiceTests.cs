using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Models;

namespace Services.Tests
{
    [TestClass]
    public class PayslipServiceTests
    {
        [TestMethod]
        public void GeneratePayslipForEmployee_TestInvalidData_ReturnsNullForPayslip()
        {
            //Arrange
            var mockTaxCalculator = new Mock<ITaxCalculatorService>();
            IPayslipService _payslipService = new PayslipService(mockTaxCalculator.Object);

            var data = new EmployeeInformation()
            {
                EmployeeFirstName = "David",
                EmployeeLastName = "Rudd",
                AnnualSalary = 60050,
                PayslipFrequency = Enums.CalculationFrequency.Monthly,
                SuperRate = 52, // << Invalid
                PayPeriod = "01 Jan-31 Jan"
            };

            //Act
            var payslip = _payslipService.GeneratePayslipForEmployee(data);

            //Assert
            Assert.IsNull(payslip);
        }

        [TestMethod]
        public void GeneratePayslipForEmployee_TestData1_ReturnExpectedData()
        {
            //Arrange
            var mockTaxCalculator = new Mock<ITaxCalculatorService>();
            mockTaxCalculator.Setup(tc => tc.CalculateIncomeTax(It.IsAny<int>(), Enums.CalculationFrequency.Monthly)).Returns(922);
            IPayslipService _payslipService = new PayslipService(mockTaxCalculator.Object);

            var data = new EmployeeInformation()
            {
                EmployeeFirstName = "David",
                EmployeeLastName = "Rudd",
                AnnualSalary = 60050,
                PayslipFrequency = Enums.CalculationFrequency.Monthly,
                SuperRate = 9,
                PayPeriod = "01 Jan-31 Jan"
            };

            var expectedResult = new PayslipInfo()
            {
                Name = "David Rudd",
                GrossIncome = 5004,
                IncomeTax = 922,
                Super = 450,
                PayPeriod = "01 Jan-31 Jan"
            };

            //Act
            var payslip = _payslipService.GeneratePayslipForEmployee(data);

            //Assert
            Assert.AreEqual(expectedResult.NetIncome, payslip.NetIncome);
            Assert.AreEqual(expectedResult.ToString(), payslip.ToString());
        }

        [TestMethod]
        public void GeneratePayslipForEmployee_TestData2_ReturnExpectedData()
        {
            //Arrange
            var mockTaxCalculator = new Mock<ITaxCalculatorService>();
            mockTaxCalculator.Setup(tc => tc.CalculateIncomeTax(It.IsAny<int>(), Enums.CalculationFrequency.Monthly)).Returns(2696);
            IPayslipService _payslipService = new PayslipService(mockTaxCalculator.Object);

            var data = new EmployeeInformation()
            {
                EmployeeFirstName = "Ryan",
                EmployeeLastName = "Chen",
                AnnualSalary = 120000,
                PayslipFrequency = Enums.CalculationFrequency.Monthly,
                SuperRate = 10,
                PayPeriod = "01 Mar-31 Mar"
            };

            var expectedResult = new PayslipInfo()
            {
                Name = "Ryan Chen",
                GrossIncome = 10000,
                IncomeTax = 2696,
                Super = 1000,
                PayPeriod = "01 Mar-31 Mar"
            };

            //Act
            var payslip = _payslipService.GeneratePayslipForEmployee(data);

            //Assert
            Assert.AreEqual(expectedResult.NetIncome, payslip.NetIncome);
            Assert.AreEqual(expectedResult.ToString(), payslip.ToString());
        }
    }
}
