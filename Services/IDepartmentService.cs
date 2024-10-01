using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IDepartmentService
    {
            Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync();
            Task<DepartmentModel> CreateDepartmentAsync(DepartmentModel model);
            Task<bool> DepartmentExistsAsync(string departmentName);
            Task<IEnumerable<EmployeeModel>> GetEmployeesByDepartmentAsync(int departmentId);
    }
}
