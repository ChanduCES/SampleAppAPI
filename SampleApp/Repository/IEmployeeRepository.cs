using SampleApp.Models;

namespace SampleApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeModel>> GetAllEmployeesAsync(EmployeeQueryParameters parameters);
        Task<EmployeeModel> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employeeModel);
        Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeModel);
        Task RemoveEmployeeAsync(EmployeeModel employee);
    }
}
