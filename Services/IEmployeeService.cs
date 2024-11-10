using Microsoft.AspNetCore.Identity;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IEmployeeService
    {
        Task<IdentityResult> RegisterEmployeeAsync(RegisterModel model);
        Task<SignInResult> LoginAsync(LoginModel model);
        Task<IdentityResult> AssignRoleAsync(RoleAssignmentModel model);
        Task<IdentityResult> AssignEmployeeToDepartmentAsync(int departmentId, string employeeId);
        Task LogoutAsync();
        Task<IEnumerable<Employees>> GetAllEmployeesAsync();
        Task BulkCreateEmployeesAsync(IEnumerable<EmployeeModel> employeeModels);
        Task<IEnumerable<EmployeeModel>> GetPagedEmployeesAsync(int pageNumber, int pageSize);
        Task SaveEmployeePhotoAsync(string employeeId, IFormFile photoFile);
    }
}
