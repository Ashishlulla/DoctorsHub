using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DoctorsHub.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Specialization> Specializations => Set<Specialization>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        
        public DbSet<ScheduleSlot> ScheduleSlots => Set<ScheduleSlot>();
        public DbSet<Bill> Bills => Set<Bill>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
