
namespace DoctorsHub.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
