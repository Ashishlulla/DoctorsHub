using DoctorsHub.Application.DTOs.Authentication;
using FluentValidation;

namespace DoctorsHub.Application.Validators.Authentication
{
    public class ForgotPasswordDtoValidator :AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator() 
        {
            RuleFor(x => x.PersonalEmail)
                .NotEmpty().WithMessage("Personal email is required.")
                .EmailAddress().WithMessage("Please enter valid email address.");
        }
    }
}
