using Services.Enums;
using Services.Models;

namespace Services
{
    public interface IPayslipService
    {
        PayslipInfo GeneratePayslipForEmployee(EmployeeInformation data);
    }
}
