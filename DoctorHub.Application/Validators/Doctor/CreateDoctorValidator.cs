using DoctorHub.Application.DTOs.Doctors;
using FluentValidation;


namespace DoctorsHub.Application.Validators.Doctor
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
    {
      public CreateDoctorValidator() 
      {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Doctor name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Qualification)
                .NotEmpty()
                .WithMessage("Qualification is required.")
                .MaximumLength(200)
                .WithMessage("Qualification cannot exceed 200 characters.");

            RuleFor(x => x.About)
                .NotEmpty()
                .WithMessage("About information is required.")
                .MaximumLength(1000)
                .WithMessage("About information cannot exceed 1000 characters.");

            RuleFor(x => x.ExperienceYears)
                .InclusiveBetween(0, 60)
                .WithMessage("Experience must be between 0 and 60 years.");

            RuleFor(x => x.ConsultationFee)
                .InclusiveBetween(100, 100000)
                .WithMessage("Consultation fee must be between ₹100 and ₹100000.");

            RuleFor(x => x.VisitDays)
                .NotEmpty()
                .WithMessage("Visit days are required.");

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Please select a specialization."); 
        }
    }
}
