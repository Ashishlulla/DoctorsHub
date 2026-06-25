using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.HasOne(r => r.Doctor)
                .WithMany(r => r.Reviews)
                .HasForeignKey(a => a.DoctorId);
        }
    }
}
