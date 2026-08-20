using DoctorsHub.Application.DTOs.Authentication;
using FluentValidation;


namespace DoctorsHub.Application.Validators.Authentication
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator() 
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Currentpassword is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MaximumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x=>x.NewPassword).WithMessage("Confirm Password must match.")
                .MaximumLength(8);

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from current password.");
        }
    }
}
