using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployeesAsync(EmployeeQueryParameters parameters);
        Task<EmployeeModel> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employeeModel);
        Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeModel);
        Task RemoveEmployeeAsync(EmployeeModel employee);
    }
}
