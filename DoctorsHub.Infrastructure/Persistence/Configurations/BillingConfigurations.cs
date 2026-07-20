using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    internal class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.ConsultationFee)
                   .HasPrecision(18, 2);

            builder.Property(b => b.AdditionalCharges)
                   .HasPrecision(18, 2);

            builder.Property(b => b.Discount)
                   .HasPrecision(18, 2);

            builder.Property(b => b.TotalAmount)
                   .HasPrecision(18, 2);

            builder.HasOne(b => b.Appointment)
                   .WithOne(a => a.Bill)
                   .HasForeignKey<Bill>(b => b.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Bills");
        }
    }
}