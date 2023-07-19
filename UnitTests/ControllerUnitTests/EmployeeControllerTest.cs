using SampleApp.Repository;
using SampleApp.Controllers;
using SampleApp.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;

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
            _employeeRepositoryMock.Setup(x => x.GetAllEmployees(queryParameters)).ReturnsAsync(employees);

            //Act
            var result = await employeeController.GetAllEmployees(queryParameters);

            //Assert
            var employeeResult = result.Result as OkObjectResult;
            employeeResult?.StatusCode.Should().Be(200);
            employeeResult.Value.Should().BeEquivalentTo(employees);
        }

        [Fact]
        public async Task ShouldThrowExceptionandReturnStatus500_WhenGetAllEmployees_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employees = _fixture.CreateMany<EmployeeModel>().ToList();
            var queryParameters = _fixture.Create<EmployeeQueryParameters>();
            var exception = _fixture.Create<Exception>();
            _employeeRepositoryMock.Setup(x => x.GetAllEmployees(queryParameters)).Throws(exception);

            //Act
            var result = await employeeController.GetAllEmployees(queryParameters);

            //Assert
            var employeeResult = result.Result as StatusCodeResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ShouldGetEmployeeModel_WhenGetEmployeeById_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);

            //Act
            var result = await employeeController.GetEmployeeById(employeeId);

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
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.GetEmployeeById(employeeId);

            //Assert
            var employeeResult = result.Result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldThrowInternalServerError_WhenGetEmployeeById_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            var exception = _fixture.Create<Exception>();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).Throws(exception);

            //Act
            var result = await employeeController.GetEmployeeById(employeeId);

            //Assert
            var employeeResult = result.Result as StatusCodeResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ShouldAddEmployee_WhenAddEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employee = _fixture.Create<EmployeeModel>();
            _employeeRepositoryMock.Setup(x => x.AddEmployee(employee)).ReturnsAsync(employee);

            //Act
            var result = await employeeController.AddEmployee(employee);

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
            _employeeRepositoryMock.Setup(x => x.AddEmployee(employee)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.AddEmployee(employee);

            //Assert
            var employeeResult = result.Result as UnprocessableEntityObjectResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task ShouldThrowInternalServerError_WhenAddEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employee = _fixture.Create<EmployeeModel>();
            var exception = _fixture.Create<Exception>();
            _employeeRepositoryMock.Setup(x => x.AddEmployee(employee)).Throws(exception);

            //Act
            var result = await employeeController.AddEmployee(employee);

            //Assert
            var employeeResult = result.Result as StatusCodeResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ShouldUpdateEmployee_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            var updatedEmployee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployee(employee)).ReturnsAsync(updatedEmployee);

            //Act
            var result = await employeeController.UpdateEmployee(employee);

            //Assert
            var employeeResult = result.Result as OkResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task ShouldThrowNotFound_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.UpdateEmployee(employee);

            //Assert
            var employeeResult = result.Result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldThrowUnprocessableEntity_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployee(employee)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.UpdateEmployee(employee);

            //Assert
            var employeeResult = result.Result as UnprocessableEntityObjectResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task ShouldThrowInternalServerError_WhenUpdateEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            var exception = _fixture.Create<Exception>();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployee(employee)).Throws(exception);

            //Act
            var result = await employeeController.UpdateEmployee(employee);

            //Assert
            var employeeResult = result.Result as StatusCodeResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ShouldRemoveEmployee_WhenRemoveEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.RemoveEmployee(employee)).ReturnsAsync(true);

            //Act
            var result = await employeeController.RemoveEmployee(employeeId);

            //Assert
            var employeeResult = result as NoContentResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ShouldReturnNotFound_WhenRemoveEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            var result = await employeeController.RemoveEmployee(employeeId);

            //Assert
            var employeeResult = result as NotFoundResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldThrowInternalServerError_WhenRemoveEmployee_IsCalled()
        {
            //Arrange
            var employeeController = CreateEmployeeController();
            var employeeId = _fixture.Create<Guid>();
            var employee = _fixture.Build<EmployeeModel>().With(x => x.EmployeeGuid, employeeId).Create();
            var exception = _fixture.Create<Exception>();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeById(employeeId)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(x => x.RemoveEmployee(employee)).Throws(exception);

            //Act
            var result = await employeeController.RemoveEmployee(employeeId);

            //Assert
            var employeeResult = result as StatusCodeResult;
            employeeResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
