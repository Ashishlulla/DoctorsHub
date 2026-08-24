

using DoctorsHub.Application.DTOs.Departments;
using FluentValidation;

namespace DoctorsHub.Application.Validators.Department
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator() 
        {
            RuleFor(d=>d.Name)
                .NotEmpty().WithMessage("Department  name cannot be empty.");
        }
    }
}
