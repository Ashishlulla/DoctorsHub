using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Specialization> Specializations => Set<Specialization>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ScheduleSlot> ScheduleSlots => Set<ScheduleSlot>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
