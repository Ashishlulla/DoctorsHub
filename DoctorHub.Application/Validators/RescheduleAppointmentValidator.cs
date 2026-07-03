using DoctorsHub.Application.DTOs.Appoitments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Validators
{
    public class RescheduleAppointmentValidator : AbstractValidator<RescheduleAppointmentDto>
    {

        public RescheduleAppointmentValidator() 
        {
            RuleFor(d => d.AppointmentDate)
                .NotEmpty().WithMessage("Please enter appointment date");

            RuleFor(d => d.StartTime)
                .NotEmpty().WithMessage("PLease enter start time");

            RuleFor(d => d.EndTime)
                .NotEmpty().WithMessage("Please enter end time");
        }
    }
}
