using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.ExtensionMethods;

namespace Services.Tests
{
    [TestClass]
    public class DecimalExtensionMethodsTests
    {
        [TestMethod]
        public void RoundToNearestInt_RoundDown_WhereDecimalPartIsLessThanPoint5()
        {
            //Arrange
            var value = (decimal)1.49;

            //Act
            value = value.RoundToNearestInt();

            //Assert
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void RoundToNearestInt_RoundUp_WhereDecimalPartIsEqualToPoint5()
        {
            //Arrange
            var value = (decimal)1.5;

            //Act
            value = value.RoundToNearestInt();

            //Assert
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void RoundToNearestInt_RoundUp_WhereDecimalPartIsMoreThanPoint5()
        {
            //Arrange
            var value = (decimal)1.51;

            //Act
            value = value.RoundToNearestInt();

            //Assert
            Assert.AreEqual(2, value);
        }
    }
}
