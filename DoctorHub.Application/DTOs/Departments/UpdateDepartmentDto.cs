

namespace DoctorsHub.Application.DTOs.Departments
{
    public class UpdateDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
