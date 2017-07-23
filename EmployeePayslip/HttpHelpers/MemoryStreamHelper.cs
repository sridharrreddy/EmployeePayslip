using System.Collections.Generic;
using System.Net.Http;

namespace EmployeePayslip.HttpHelpers
{
    public class MemoryStreamHelper : IMemoryStreamHelper
    {
        public List<MultipartFileInfo> LoadFileInfo(HttpContent httpContent)
        {
            var fileInfoList = new List<MultipartFileInfo>();
            var memoryStreamProvider = new MultipartMemoryStreamProvider();
            httpContent.ReadAsMultipartAsync(memoryStreamProvider);
            foreach (var content in memoryStreamProvider.Contents)
            {
                fileInfoList.Add(
                    new MultipartFileInfo()
                    {
                        FileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty),
                        Stream = content.ReadAsStreamAsync().Result
                    }
                );
            }
            return fileInfoList;
        }
    }
}