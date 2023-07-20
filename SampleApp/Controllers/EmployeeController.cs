using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Repository;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployeesAsync([FromQuery] EmployeeQueryParameters parameters)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync(parameters);
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<List<EmployeeModel>>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee?.EmployeeGuid != null)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> AddEmployeeAsync([FromBody] EmployeeModel employee)
        {
            var newEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            if (newEmployee?.EmployeeGuid != null)
            {
                return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = newEmployee.EmployeeGuid }, newEmployee);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeModel>> UpdateEmployeeAsync([FromBody] EmployeeModel employee)
        {
            var currentEmployee = await _employeeRepository.GetEmployeeByIdAsync(employee.EmployeeGuid);
            if (currentEmployee != null)
            {
                var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
                if (updatedEmployee != null)
                {
                    return Ok();
                }
                else
                {
                    return UnprocessableEntity(employee);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee != null)
            {
                await _employeeRepository.RemoveEmployeeAsync(employee);
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
