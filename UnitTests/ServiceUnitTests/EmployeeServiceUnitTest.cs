using AutoMapper;
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
    }
}
