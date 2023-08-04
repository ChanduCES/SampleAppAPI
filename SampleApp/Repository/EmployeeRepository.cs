using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetches the list of employees from the Employees table.
        /// </summary>
        /// <returns>List of employees.</returns>
        public async Task<List<Employee>> GetAllEmployeesAsync(EmployeeQueryParameters parameters)
        {
            List<Employee> employees = await _context.Employees.FromSql($"Usp_GetEmployees @pageNo ={parameters.CurrentPage}, @pageSize ={parameters.PageSize}, @searchText={parameters.SearchString}").ToListAsync();
            return employees;
        }

        /// <summary>
        /// Fetches the employee model for the given Id.
        /// </summary>
        /// <param name="id">Id for the employee</param>
        /// <returns>Employee model with the given Id.</returns>
        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return employee;
        }

        /// <summary>
        /// Add the new employee to the Employee table.
        /// </summary>
        /// <param name="employeeModel">Employee to be added.</param>
        /// <returns>Employee model of the new Employee.</returns>
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var newEmployee = _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return newEmployee.Entity;

        }

        /// <summary>
        /// Updates the employee details to the Employee table.
        /// </summary>
        /// <param name="employeeModel">Employee to be added.</param>
        /// <returns>Employee model of the new Employee.</returns>
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        /// <summary>
        /// Removes the employee from the Employee table.
        /// </summary>
        /// <param name="employeeId">Employee ID of the employee to be removed.</param>
        /// <returns>True if employee is removed.</returns>
        public async Task RemoveEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
