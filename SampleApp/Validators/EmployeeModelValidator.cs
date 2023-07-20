using FluentValidation;
using SampleApp.Models;

namespace SampleApp.Validators
{
    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Role).IsInEnum().NotEmpty();
        }
    }
}
