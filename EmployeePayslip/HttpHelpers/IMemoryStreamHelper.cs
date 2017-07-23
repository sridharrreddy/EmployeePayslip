using System.Collections.Generic;
using System.Net.Http;

namespace EmployeePayslip.HttpHelpers
{
    public interface IMemoryStreamHelper
    {
        List<MultipartFileInfo> LoadFileInfo(HttpContent httpContent);
    }
}
