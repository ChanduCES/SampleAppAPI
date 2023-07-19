using AutoMapper;
using SampleApp.Data;
using SampleApp.Models;
using System.Globalization;

namespace SampleApp.Profiles
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<EmployeeModel, Employee>().ForMember(x => x.Salary, x => x.MapFrom(src => GetSalaryAsInt(src)));
            CreateMap<Employee, EmployeeModel>().ForMember(x => x.Salary, x => x.MapFrom(src => GetSalaryAsString(src)));
        }

        private string GetSalaryAsString(Employee employee)
        {
            return employee.Salary.ToString("N0");
        }

        private double GetSalaryAsInt(EmployeeModel employee)
        {
            return double.Parse(employee.Salary, NumberStyles.AllowThousands);
        }
    }
}
