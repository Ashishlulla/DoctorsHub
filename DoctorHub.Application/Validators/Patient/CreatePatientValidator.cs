using DoctorsHub.Application.DTOs.Patients;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Validators.Patient
{
    public class CreatePatientValidator :AbstractValidator<CreatePatientDto>
    {
        public CreatePatientValidator() 
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Today)
                .WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Please select a valid gender.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Please enter a valid 10-digit Indian mobile number.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(250)
                .WithMessage("Address cannot exceed 250 characters.");

            RuleFor(x => x.BloodGroup)
                .NotEmpty().WithMessage("Blood Group is required.")
                .Must(bg => new[]
                { "A+","A-","B+","B-","AB+","AB-","O+","O-" }.Contains(bg)).WithMessage("Please select a valid blood group.");
        }
    }
}
