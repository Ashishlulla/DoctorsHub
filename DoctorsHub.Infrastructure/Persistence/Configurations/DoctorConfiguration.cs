using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Qualification)
                .HasMaxLength(200);

            builder.Property(d => d.About)
                .HasMaxLength(1000);

            builder.Property(d => d.ConsultationFee)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
