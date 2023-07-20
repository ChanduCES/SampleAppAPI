using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetches the list of employees from the Employees table.
        /// </summary>
        /// <returns>List of employees.</returns>
        public async Task<List<EmployeeModel>> GetAllEmployeesAsync(EmployeeQueryParameters parameters)
        {
            List<Employee> employees = await _context.Employees.Where(x =>
                                                                    x.IsActive.Equals(parameters.Status) &&
                                                                    (string.IsNullOrWhiteSpace(parameters.SearchString) ||
                                                                    x.Name.Contains(parameters.SearchString) ||
                                                                    x.Salary.ToString().Contains(parameters.SearchString)) &&
                                                                    (parameters.InitialDate == default || x.JoiningDate >= parameters.InitialDate) &&
                                                                    (parameters.FinalDate == default || x.JoiningDate <= parameters.FinalDate)).ToListAsync();

            employees = employees.Skip((parameters.CurrentPage - 1) * parameters.PageSize).Take(parameters.PageSize)?.ToList();
            return _mapper.Map<List<EmployeeModel>>(employees);
        }

        /// <summary>
        /// Fetches the employee model for the given Id.
        /// </summary>
        /// <param name="id">Id for the employee</param>
        /// <returns>Employee model with the given Id.</returns>
        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeGuid.Equals(id));
            return _mapper.Map<EmployeeModel>(employee);
        }

        /// <summary>
        /// Add the new employee to the Employee table.
        /// </summary>
        /// <param name="employeeModel">Employee to be added.</param>
        /// <returns>Employee model of the new Employee.</returns>
        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employeeModel)
        {
            Employee employee = _mapper.Map<Employee>(employeeModel);
            var newEmployee = _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeModel>(newEmployee.Entity);

        }

        /// <summary>
        /// Updates the employee details to the Employee table.
        /// </summary>
        /// <param name="employeeModel">Employee to be added.</param>
        /// <returns>Employee model of the new Employee.</returns>
        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeModel)
        {
            Employee employee = _mapper.Map<Employee>(employeeModel);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeModel>(employee);
        }

        /// <summary>
        /// Removes the employee from the Employee table.
        /// </summary>
        /// <param name="employeeId">Employee ID of the employee to be removed.</param>
        /// <returns>True if employee is removed.</returns>
        public async Task<bool> RemoveEmployeeAsync(EmployeeModel employeeModel)
        {
            Employee employee = _mapper.Map<Employee>(employeeModel);
            var a = _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
