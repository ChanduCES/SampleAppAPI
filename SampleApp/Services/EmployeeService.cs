using AutoMapper;
using SampleApp.Data;
using SampleApp.Models;
using SampleApp.Repository;

namespace SampleApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) 
        { 
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employeeModel)
        {
            var employee = _mapper.Map<Employee>(employeeModel);
            var newEmployeeModel = await _employeeRepository.AddEmployeeAsync(employee);
            return _mapper.Map<EmployeeModel>(newEmployeeModel);
        }

        public async Task<List<EmployeeModel>> GetAllEmployeesAsync(EmployeeQueryParameters parameters)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync(parameters);
            return _mapper.Map<List<EmployeeModel>>(employees);
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id)
        {
            var employeeModel = await _employeeRepository.GetEmployeeByIdAsync(id);
            return _mapper.Map<EmployeeModel>(employeeModel);
        }

        public async Task RemoveEmployeeAsync(EmployeeModel employeeModel)
        {
            var employee = _mapper.Map<Employee>(employeeModel);
            await _employeeRepository.RemoveEmployeeAsync(employee);
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeModel)
        {
            var employee = _mapper.Map<Employee>(employeeModel);
            var updatedEmployee =  await _employeeRepository.UpdateEmployeeAsync(employee);
            return _mapper.Map<EmployeeModel>(employee);
        }
    }
}
