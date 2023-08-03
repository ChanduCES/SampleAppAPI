using SampleApp.Data;
using SampleApp.Models;
using SampleApp.Repository;

namespace SampleApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly Repository.IEmployeeRepository _employeeRepository;

        public EmployeeService(Repository.IEmployeeRepository employeeRepository) 
        { 
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employeeModel) => await _employeeRepository.AddEmployeeAsync(employeeModel);

        public async Task<List<EmployeeModel>> GetAllEmployeesAsync(EmployeeQueryParameters parameters) => await _employeeRepository.GetAllEmployeesAsync(parameters);

        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id) => await _employeeRepository.GetEmployeeByIdAsync(id);

        public async Task RemoveEmployeeAsync(EmployeeModel employee) => await _employeeRepository.RemoveEmployeeAsync(employee);

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeModel) => await _employeeRepository.UpdateEmployeeAsync(employeeModel);
    }
}
