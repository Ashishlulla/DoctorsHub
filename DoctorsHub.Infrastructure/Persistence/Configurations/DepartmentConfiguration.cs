using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsHub.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
          
            builder.HasMany(d=>d.Doctors)
                .WithMany(d=>d.Departments)
                .UsingEntity(j => j.ToTable("DoctorDepartments"));
        }
    }
}
