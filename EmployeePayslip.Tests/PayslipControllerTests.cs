using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using EmployeePayslip.api;
using Services;
using Moq;
using System.Web.Http;
using System.Text;
using System.Net.Http.Headers;
using System.IO;
using EmployeePayslip.HttpHelpers;
using System.Collections.Generic;
using System.Net;
using Services.Models;

namespace EmployeePayslip.Tests
{
    [TestClass]
    public class PayslipControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Upload_UnsupportedMediaType_Throw_HttpResponseException()
        {
            //Arrange
            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = new StringContent("plain string");

            //Act
            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            var mockTaxCalculator = new Mock<IPayslipService>();
            var controller = new PayslipController(mockTaxCalculator.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;

            //Assert
            controller.Upload();
        }

        [TestMethod]
        public void Upload_NoFilesUploaded_ReturnBadRequest()
        {
            //Arrange
            var file = new StreamContent(new MemoryStream(new Byte[100]));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            file.Headers.ContentDisposition.FileName = "image.jpeg";

            var content = new MultipartFormDataContent();
            content.Add(file);

            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = content;

            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            mockMemoryStreamHelper.Setup(ms => ms.LoadFileInfo(It.IsAny<HttpContent>())).Returns((List<MultipartFileInfo>)null);
            var mockTaxCalculator = new Mock<IPayslipService>();

            //Act
            var controller = new PayslipController(mockTaxCalculator.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;
            var response = controller.Upload();

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Method only acccepts 1 csv file", response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void Upload_MoreThan1FilesUploaded_ReturnBadRequest()
        {
            //Arrange
            var file = new StreamContent(new MemoryStream(new Byte[100]));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            file.Headers.ContentDisposition.FileName = "image.jpeg";

            var content = new MultipartFormDataContent();
            content.Add(file);

            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = content;

            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            mockMemoryStreamHelper.Setup(ms => ms.LoadFileInfo(It.IsAny<HttpContent>())).Returns(new List<MultipartFileInfo>() { new MultipartFileInfo() { }, new MultipartFileInfo() { } });
            var mockTaxCalculator = new Mock<IPayslipService>();

            //Act
            var controller = new PayslipController(mockTaxCalculator.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;
            var response = controller.Upload();

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Method only acccepts 1 csv file", response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void Upload_NonCsvFile_ReturnBadRequest()
        {
            //Arrange
            var file = new StreamContent(new MemoryStream(new Byte[100]));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            file.Headers.ContentDisposition.FileName = "image.jpeg";

            var content = new MultipartFormDataContent();
            content.Add(file);

            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = content;

            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            mockMemoryStreamHelper.Setup(ms => ms.LoadFileInfo(It.IsAny<HttpContent>())).Returns(new List<MultipartFileInfo>() { new MultipartFileInfo() { FileName = "image.jpg" } });
            var mockTaxCalculator = new Mock<IPayslipService>();

            //Act
            var controller = new PayslipController(mockTaxCalculator.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;
            var response = controller.Upload();

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Method only acccepts csv file", response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void Upload_CsvFileWithBadData_ReturnsBadRequest()
        {
            //Arrange
            var file = new StreamContent(new MemoryStream(new Byte[100]));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            file.Headers.ContentDisposition.FileName = "image.jpeg";

            var content = new MultipartFormDataContent();
            content.Add(file);

            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = content;

            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"first name,last name,annual salary,,payment start date
David,Rudd,60050,9%"));
            mockMemoryStreamHelper.Setup(ms => ms.LoadFileInfo(It.IsAny<HttpContent>())).Returns(new List<MultipartFileInfo>() { new MultipartFileInfo() { FileName = "info.csv", Stream = stream } });
            var mockTaxCalculator = new Mock<IPayslipService>();

            //Act
            var controller = new PayslipController(mockTaxCalculator.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;
            var response = controller.Upload();

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(response.Content.ReadAsStringAsync().Result.StartsWith("The CSV appears to be corrupt"));
        }

        [TestMethod]
        public void Upload_CsvFileWith1DataRow_ReturnsCsv()
        {
            //Arrange
            var file = new StreamContent(new MemoryStream(new Byte[100]));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            file.Headers.ContentDisposition.FileName = "image.jpeg";

            var content = new MultipartFormDataContent();
            content.Add(file);

            var requestMsg = new HttpRequestMessage();
            requestMsg.Content = content;

            var mockMemoryStreamHelper = new Mock<IMemoryStreamHelper>();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"first name,last name,annual salary,super rate (%),payment start date
David,Rudd,60050,9%,01 March - 31 March"));
            mockMemoryStreamHelper.Setup(ms => ms.LoadFileInfo(It.IsAny<HttpContent>())).Returns(new List<MultipartFileInfo>() { new MultipartFileInfo() { FileName = "info.csv", Stream = stream } });

            var mockPayslipService = new Mock<IPayslipService>();
            mockPayslipService.Setup(ps => ps.GeneratePayslipForEmployee(It.IsAny<EmployeeInformation>())).Returns(new PayslipInfo());

            //Act
            var controller = new PayslipController(mockPayslipService.Object, mockMemoryStreamHelper.Object);
            controller.Request = requestMsg;
            var response = controller.Upload();

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/csv", response.Content.Headers.ContentType.MediaType.ToString());
        }
    }
}
