using SampleApp.Repository;
using SampleApp.Controllers;
using SampleApp.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;
using SampleApp.Enums;

namespace UnitTests.ControllerUnitTests
{
    public class EmployeeControllerTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

        public EmployeeControllerTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _employeeRepositoryMock = _mockRepository.Create<IEmployeeRepository>();
        }

        private EmployeeController CreateEmployeeController()
        {
            return new EmployeeController(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldGetEmployeeList_WhenGetAllEmployees_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employees = _fixture.CreateMany<EmployeeModel>().ToList();
            var queryParameters = _fixture.Create<EmployeeQueryParameters>();
            _employeeRepositoryMock.Setup(x => x.GetAllEmployeesAsync(queryParameters)).ReturnsAsync(employees);

            //Act
            var result = await employeeController.GetAllEmployeesAsync(queryParameters);

            //Assert
            var employeeResult = result.Result as OkObjectResult;
            employeeResult?.StatusCode.Should().Be(200);
            employeeResult.Value.Should().BeEquivalentTo(employees);
        }

        
        [Fact]
        public async Task ShouldGetEmployeeModel_WhenGetEmployeeById_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            //Act
            var result = await employeeController.GetEmployeeByIdAsync(employeeId);

            //Assert
            var employeeResult = result.Result as OkObjectResult;
            employeeResult?.StatusCode.Should().Be(200);
            employeeResult?.Value.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task ShouldReturnNotFound_WhenGetEmployeeById_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.GetEmployeeByIdAsync(employeeId);

            //Assert
            var employeeResult = result.Result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be(Status404NotFound);
        }

        
        [Fact]
        public async Task ShouldAddEmployee_WhenAddEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employee = _fixture.Create<EmployeeModel>();
            _employeeRepositoryMock.Setup(x => x.AddEmployeeAsync(employee)).ReturnsAsync(employee);

            //Act
            var result = await employeeController.AddEmployeeAsync(employee);

            //Assert
            var employeeResult = result.Result as CreatedAtActionResult;
            employeeResult?.StatusCode.Should().Be(201);
            employeeResult.Value.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task ShouldReturnUnprocessableEntity_WhenAddEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employee = _fixture.Create<EmployeeModel>();
            _employeeRepositoryMock.Setup(x => x.AddEmployeeAsync(employee)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.AddEmployeeAsync(employee);

            //Assert
            var employeeResult = result.Result as UnprocessableEntityObjectResult;
            employeeResult?.StatusCode.Should().Be(Status422UnprocessableEntity);
        }
                
        [Fact]
        public async Task ShouldUpdateEmployee_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            var updatedEmployee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployeeAsync(employee)).ReturnsAsync(updatedEmployee);

            //Act
            var result = await employeeController.UpdateEmployeeAsync(employee);

            //Assert
            var employeeResult = result.Result as OkResult;
            employeeResult?.StatusCode.Should().Be(Status200OK);
        }

        [Fact]
        public async Task ShouldThrowNotFound_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.UpdateEmployeeAsync(employee);

            //Assert
            var employeeResult = result.Result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be(Status404NotFound);
        }

        [Fact]
        public async Task ShouldThrowUnprocessableEntity_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployeeAsync(employee)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.UpdateEmployeeAsync(employee);

            //Assert
            var employeeResult = result.Result as UnprocessableEntityObjectResult;
            employeeResult?.StatusCode.Should().Be(Status422UnprocessableEntity);
        }

        [Fact]
        public async Task ShouldRemoveEmployee_WhenRemoveEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.RemoveEmployeeAsync(employee)).Returns(Task.CompletedTask);

            //Act
            var result = await employeeController.RemoveEmployeeAsync(employeeId);

            //Assert
            var employeeResult = result as NoContentResult;
            employeeResult?.StatusCode.Should().Be(Status204NoContent);
        }

        [Fact]
        public async Task ShouldReturnNotFound_WhenRemoveEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.RemoveEmployeeAsync(employeeId);

            //Assert
            var employeeResult = result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be(Status404NotFound);
        }
    }
}
