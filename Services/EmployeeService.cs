using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyContext _context;
        private readonly UserManager<Employees> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Employees> _signInManager;

        public EmployeeService(MyContext context, UserManager<Employees> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Employees> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> RegisterEmployeeAsync(RegisterModel model)
        {
            var user = new Employees { UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return result;  
            }

            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            await _userManager.AddToRoleAsync(user, "Employee");

            return IdentityResult.Success;
        }

        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

            return result;
        }

        public async Task<IdentityResult> AssignRoleAsync(RoleAssignmentModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found"
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            return roleResult;
        }

        public async Task<IdentityResult> AssignEmployeeToDepartmentAsync(int departmentId, string employeeId)
        {
            var user = await _userManager.FindByIdAsync(employeeId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Employee not found"
                });
            }
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "The specified department does not exist"
                });
            }
            user.DeptID = departmentId;
            _context.Employees.Update(user);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
