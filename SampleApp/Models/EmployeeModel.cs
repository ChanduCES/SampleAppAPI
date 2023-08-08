using SampleApp.Enums;

namespace SampleApp.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Roles Role { get; set; }

        public DateTime JoiningDate { get; set; }

        public bool IsActive { get; set; }

        public string Salary { get; set; }
    }
}
