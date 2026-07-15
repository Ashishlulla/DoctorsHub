using DoctorsHub.Application.DTOs.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Validators.Authentication
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator() 
        {
            RuleFor(l => l.Email)
                .NotEmpty().WithMessage("Email cannot be blank")
                .EmailAddress().WithMessage("Invalid Email format.");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}
