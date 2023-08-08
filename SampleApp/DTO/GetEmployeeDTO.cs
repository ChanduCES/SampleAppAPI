using SampleApp.Enums;

namespace SampleApp.DTO
{
    public class GetEmployeeDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Roles Role { get; set; }
    }
}
