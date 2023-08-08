using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;
using SampleApp.Repository;

namespace UnitTests.RepositoryTests
{
    public class EmployeeRepositoryUnitTest : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly EmployeeContext _employeeContext;

        public EmployeeRepositoryUnitTest()
        {
            _fixture = new Fixture();

            var options = new DbContextOptionsBuilder<EmployeeContext>()
                                .UseInMemoryDatabase(databaseName: "EmployeeDBTests")
                                .Options;

            _employeeContext = new EmployeeContext(options);
            _employeeContext.Database.EnsureCreated();
        }

        private EmployeeRepository CreateEmployeeRepository()
        {
            return new EmployeeRepository(_employeeContext);
        }

        [Fact]
        public async Task ShouldReturnEmployeeList_When_GetAllEmployeesAsync_IsCalled()
        {
            //Arrange
            var employeeRepository = CreateEmployeeRepository();
            var employee = _fixture.CreateMany<Employee>();
            _employeeContext.Employees.AddRange(employee);
            _employeeContext.SaveChanges();

            //Act
            var result = await employeeRepository.GetAllEmployeesAsync(new EmployeeQueryParameters());

            //Assert
            result.Count().Should().Be(employee.Count());
        }

        [Fact]
        public async Task ShouldAddNewEmployeeAndGetTheSameEmployeeBack_When_AddEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeRepository = CreateEmployeeRepository();
            var employees = _fixture.Create<Employee>();

            //Act
            var result = await employeeRepository.AddEmployeeAsync(employees);
            var employeeAdded = await employeeRepository.GetEmployeeByIdAsync(result.Id);

            //Assert
            employeeAdded.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task ShouldUpdateEmployee_When_UpdateEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeRepository = CreateEmployeeRepository();
            var employee = _fixture.Create<Employee>();

            //Act
            var result = await employeeRepository.AddEmployeeAsync(employee);
            var employeeAdded = await employeeRepository.GetEmployeeByIdAsync(result.Id);
            employeeAdded.IsActive = !employeeAdded.IsActive;
            employeeAdded.Salary = _fixture.Create<double>();
            var employeeUpdated = await employeeRepository.UpdateEmployeeAsync(employeeAdded);

            //Assert
            employeeUpdated.Should().BeEquivalentTo(employeeAdded);
            employeeUpdated.IsActive.Should().Be(employeeAdded.IsActive);
            employeeUpdated.Salary.Should().Be(employeeAdded.Salary);
        }

        [Fact]
        public async Task ShouldRemoveEmployee_When_RemoveEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeRepository = CreateEmployeeRepository();
            var employees = _fixture.Create<Employee>();

            //Act
            var result = await employeeRepository.AddEmployeeAsync(employees);
            var employeeAdded = await employeeRepository.GetEmployeeByIdAsync(result.Id);
            await employeeRepository.RemoveEmployeeAsync(employeeAdded);
            var removedEmployee = await employeeRepository.GetEmployeeByIdAsync(result.Id);

            //Assert
            employeeAdded.Should().BeEquivalentTo(result);
            removedEmployee.Should().Be(null);
        }

        public void Dispose()
        {
            _employeeContext.Database.EnsureDeleted();
            _employeeContext.Dispose();
        }
    }
}
