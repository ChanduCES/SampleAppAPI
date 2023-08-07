using AutoMapper;
using Moq;
using SampleApp.Data;
using SampleApp.Models;
using SampleApp.Profiles;
using SampleApp.Repository;
using SampleApp.Services;

namespace UnitTests.ServiceUnitTests
{
    public class EmployeeServiceUnitTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly IMapper _mapper;

        public EmployeeServiceUnitTest()
        {
            _fixture = new Fixture();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMapper()));
            _mapper = new Mapper(configuration);
        }

        private EmployeeService CreateEmployeeService()
        {
            return new EmployeeService(_employeeRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task ShouldGetAllEmployees_When_GetAllEmployeesAsync_IsCalled()
        {
            //Arrange
            var employeeService = CreateEmployeeService();
            var employees = _fixture.CreateMany<Employee>().ToList();
            var queryParams = new EmployeeQueryParameters();
            _employeeRepositoryMock.Setup(x => x.GetAllEmployeesAsync(queryParams)).ReturnsAsync(employees);
            var employeeModel = _mapper.Map<List<EmployeeModel>>(employees);

            //Act
            var result = await employeeService.GetAllEmployeesAsync(queryParams);

            //Assert
            result.Count().Should().Be(employeeModel.Count());
            result.GetType().Should().Be(typeof(List<EmployeeModel>));
            result.Should().BeEquivalentTo(employeeModel);
        }


        [Fact]
        public async Task ShouldGetEmployee_When_GetEmployeeByIdAsync_IsCalled()
        {
            //Arrange
            var employeeService = CreateEmployeeService();
            var employee = _fixture.Create<Employee>();
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(employee.Id)).ReturnsAsync(employee);
            var employeeModel = _mapper.Map<EmployeeModel>(employee);

            //Act
            var result = await employeeService.GetEmployeeByIdAsync(employee.Id);

            //Assert
            result.GetType().Should().Be(typeof(EmployeeModel));
            result.Should().BeEquivalentTo(employeeModel);
            employee.Id.Should().Be(result.Id);
        }

        [Fact]
        public async Task ShouldAddEmployee_When_AddEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeService = CreateEmployeeService();
            var employee = _fixture.Create<Employee>();
            var employeeModel = _mapper.Map<EmployeeModel>(employee);
            _employeeRepositoryMock.Setup(x => x.AddEmployeeAsync(It.IsAny<Employee>())).ReturnsAsync(employee);

            //Act
            var result = await employeeService.AddEmployeeAsync(employeeModel);

            //Assert
            result.Should().BeEquivalentTo(employeeModel);
            result.GetType().Should().Be(typeof(EmployeeModel));
        }

        [Fact]
        public async Task ShoulUpdateEmployee_When_UpdateEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeService = CreateEmployeeService();
            var employee = _fixture.Create<Employee>();
            var employeeModel = _mapper.Map<EmployeeModel>(employee);
            _employeeRepositoryMock.Setup(x => x.UpdateEmployeeAsync(It.IsAny<Employee>())).ReturnsAsync(employee);

            //Act
            var result = await employeeService.UpdateEmployeeAsync(employeeModel);

            //Assert
            result.Should().BeEquivalentTo(employeeModel);
            result.GetType().Should().Be(typeof(EmployeeModel));
        }

        [Fact]
        public async Task ShouldRemoveEmployee_When_RemoveEmployeeAsync_IsCalled()
        {
            //Arrange
            var employeeService = CreateEmployeeService();
            _employeeRepositoryMock.Setup(x => x.RemoveEmployeeAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            //Act
            await employeeService.RemoveEmployeeAsync(It.IsAny<EmployeeModel>());

            //Assert
            _employeeRepositoryMock.Verify(x=> x.RemoveEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            _employeeRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
