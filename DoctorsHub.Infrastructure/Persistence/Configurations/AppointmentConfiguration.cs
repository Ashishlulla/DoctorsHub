using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(s => s.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(d=>d.DoctorId);
        }
    }
}
