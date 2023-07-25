using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;
using SampleApp.Repository;

namespace UnitTests.RepositoryUnitTests
{
    public class EmployeeRepositoryUnitTest : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly EmployeeContext _employeeContext;
        private readonly Mock<IMapper> _mapperMock;

        public EmployeeRepositoryUnitTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mapperMock = _mockRepository.Create<IMapper>();

            var options = new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _employeeContext = new EmployeeContext(options);
            _employeeContext.Database.EnsureCreated();

        }

        private EmployeeRepository CreateEmployeeRepository()
        {
            return new EmployeeRepository(_employeeContext, _mapperMock.Object);
        }

        [Fact]
        public async Task ShouldReturnEmployeeList_When_GetAllEmployeesAsync_IsCalled()
        {
            //Arrange
            var employeeRepository = CreateEmployeeRepository();
            var employees = _fixture.CreateMany<Employee>();
            _employeeContext.Employees.AddRange(employees);
            _employeeContext.SaveChanges();

            //Act
            var result = await employeeRepository.GetAllEmployeesAsync(new EmployeeQueryParameters());

            //Assert
            result.Count().Should().Be(employees.Count());
        }

        public void Dispose()
        {
            _employeeContext.Database.EnsureDeleted();
            _employeeContext.Dispose();
        }
    }
}
