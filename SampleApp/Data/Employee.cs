using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp.Data
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EmployeeGuid { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
        public double Salary { get; set; }
    }
}
