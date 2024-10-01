using Microsoft.AspNetCore.Identity;
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
    }
}
