using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Repository;
using SampleApp.Services;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet]
        public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployeesAsync([FromQuery] EmployeeQueryParameters parameters)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(parameters);
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetEmployeeByIdAsync))]
        public async Task<ActionResult<List<EmployeeModel>>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee?.Id != null)
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
            var newEmployee = await _employeeService.AddEmployeeAsync(employee);
            if (newEmployee?.Id != null)
            {
                return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = newEmployee.Id }, newEmployee);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeModel>> UpdateEmployeeAsync([FromBody] EmployeeModel employee)
        {
            var currentEmployee = await _employeeService.GetEmployeeByIdAsync(employee.Id);
            if (currentEmployee != null)
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employee);
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
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee != null)
            {
                await _employeeService.RemoveEmployeeAsync(employee);
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
