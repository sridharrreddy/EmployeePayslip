using EmployeePayslip.HttpHelpers;
using LumenWorks.Framework.IO.Csv;
using Services;
using Services.ExtensionMethods;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace EmployeePayslip.api
{
    public class PayslipController : ApiController
    {
        private IMemoryStreamHelper _fileStreamHelper;
        private IPayslipService _payslipService;
        public PayslipController(IPayslipService payslipService, IMemoryStreamHelper fileStreamHelper)
        {
            _payslipService = payslipService;
            _fileStreamHelper = fileStreamHelper;
        }

        [HttpPost]
        public HttpResponseMessage Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Load csv stream into objects
            var fileInfoList = _fileStreamHelper.LoadFileInfo(Request.Content);
            if (fileInfoList == null || fileInfoList.Count != 1)
            {
                return ReturnBadRequest("Method only acccepts 1 csv file");
            }

            var fileInfo = fileInfoList.First();
            //End bad request with 400
            if (!fileInfo.FileName.EndsWith(".csv"))
            {
                return ReturnBadRequest("Method only acccepts csv file");
            }

            string errors;
            var employeesInfo = ParseInput(fileInfo.Stream, out errors);
            //End bad request with 400
            if (!string.IsNullOrEmpty(errors))
            {
                return ReturnBadRequest(errors);
            }

            //Generate payslips
            var payslipInfoList = new List<PayslipInfo>();
            foreach (var employeeInfo in employeesInfo)
            {
                payslipInfoList.Add(_payslipService.GeneratePayslipForEmployee(employeeInfo));
            }

            //Generate response
            var response = Request.CreateResponse(HttpStatusCode.OK);
            DataTable payslipTable = payslipInfoList.ToDataTable();
            response.Content = new ByteArrayContent(Encoding.ASCII.GetBytes(payslipTable.ToCsv()));
            response.Content.Headers.Add("Content-Disposition", "attachment;filename=Payslips.csv");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            return response;
        }

        private List<EmployeeInformation> ParseInput(Stream stream, out string errors)
        {
            try
            {
                DataTable employeeData = new DataTable();
                using (CsvReader csvReader = new CsvReader(new StreamReader(stream), true))
                {
                    employeeData.Load(csvReader);
                }
                errors = null;

                return EmployeeInformation.LoadFromDataTable(employeeData);
            }
            catch (Exception ex)
            {
                errors = ex.Message;
                return null;
            }
        }

        private HttpResponseMessage ReturnBadRequest(string errors)
        {
            var failedResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
            failedResponse.Content = new StringContent(errors);
            return failedResponse;
        }
    }
}