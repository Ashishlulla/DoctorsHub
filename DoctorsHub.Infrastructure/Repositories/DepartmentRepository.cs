using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace DoctorsHub.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        //Private Feilds
        private readonly ApplicationDbContext _db;

        //Constructor
        public DepartmentRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            await _db.Departments.AddAsync(department);
            await _db.SaveChangesAsync();

            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(Department department)
        {
            Department? departmentToDelete = await _db.Departments.FirstOrDefaultAsync(d => d.Id == department.Id);
            if (departmentToDelete == null)
            {
                return false;
            }

            _db.Departments.Remove(departmentToDelete);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            Department? department = await _db.Departments.FirstOrDefaultAsync(d => d.Id == id);
            return department ?? null;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _db.Departments.ToListAsync();
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            Department? departmentToUpdate = await _db.Departments.FirstOrDefaultAsync(d => d.Id == department.Id);

            if (departmentToUpdate == null) 
            {
                return null!;
            }

            departmentToUpdate.Name = department.Name;
            departmentToUpdate.Description = department.Description;

            _db.Departments.Update(departmentToUpdate!);
            
            await _db.SaveChangesAsync();

            return departmentToUpdate;
        }
    }
}
