using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    public class ScheduleSlotConfiguration : IEntityTypeConfiguration<ScheduleSlot>
    {
        public void Configure(EntityTypeBuilder<ScheduleSlot> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Doctor)
                   .WithMany(d => d.ScheduleSlots)
                   .HasForeignKey(s => s.DoctorId);
        }
    }
}