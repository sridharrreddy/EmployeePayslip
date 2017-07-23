using System.IO;

namespace EmployeePayslip.HttpHelpers
{
    public class MultipartFileInfo
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }
}