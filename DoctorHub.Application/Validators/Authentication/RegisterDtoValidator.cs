using DoctorsHub.Application.DTOs.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Validators.Authentication
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator() 
        {
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("First Name cannot be empty");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Last Name cannot be empty");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("password cannot be empty")
                .MinimumLength(8);

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password cannot be empty")
                .Equal(r => r.Password).WithMessage("Confirm password should match with password");



        }
    }
}
