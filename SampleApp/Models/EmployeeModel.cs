using SampleApp.Enums;

namespace SampleApp.Models
{
    public class EmployeeModel
    {
        /// <summary>
        /// Employee ID of the Employee.
        /// </summary>
        public Guid EmployeeGuid { get; set; }

        /// <summary>
        /// Full name of the Employee.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Role of the Employee.
        /// </summary>
        public Roles? Role { get; set; }

        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
        public string Salary { get; set; }
    }
}
