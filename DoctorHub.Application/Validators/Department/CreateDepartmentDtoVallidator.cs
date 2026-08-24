using DoctorsHub.Application.DTOs.Departments;
using FluentValidation;

namespace DoctorsHub.Application.Validators.Department
{
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentDtoValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty()
                .WithMessage("Department name cannot be empty.");
        }
    }
}