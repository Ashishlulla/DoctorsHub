using DoctorsHub.Application.DTOs.Appoitments;
using FluentValidation;

namespace DoctorsHub.Application.Validators
{
    public class UpdateAppointmentValidator :AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentValidator() 
        {
            RuleFor(x => x.Id)
             .GreaterThan(0)
             .WithMessage("Appointment Id cannot be empty");

            RuleFor(x => x.DoctorId)
              .GreaterThan(0)
              .WithMessage("Please select a doctor.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("Please select a patient.");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("Appointment date is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.");

            RuleFor(x => x)
                .Must(x => x.EndTime > x.StartTime)
                .WithMessage("End time must be later than start time.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required.")
                .MaximumLength(250)
                .WithMessage("Reason cannot exceed 250 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
