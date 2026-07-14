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
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit.");

            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password cannot be empty")
                .Equal(r => r.Password).WithMessage("Confirm Password should match with Password");

        }
    }
}
