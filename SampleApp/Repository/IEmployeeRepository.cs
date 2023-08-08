using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync(EmployeeQueryParameters parameters);
        Task<Employee> GetEmployeeByIdAsync(Guid id);
        Task<Employee> AddEmployeeAsync(Employee employeeModel);
        Task<Employee> UpdateEmployeeAsync(Employee employeeModel);
        Task RemoveEmployeeAsync(Employee employee);
    }
}
