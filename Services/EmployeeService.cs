using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Repositories;
namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyContext _context;
        private readonly UserManager<Employees> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Employees> _signInManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileService _fileService;

        public EmployeeService(MyContext context,
            UserManager<Employees> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<Employees> signInManager,
            IEmployeeRepository employeeRepository,
            IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _employeeRepository = employeeRepository;
            _fileService = fileService;
        }
        public async Task<IEnumerable<Employees>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
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
        public async Task BulkCreateEmployeesAsync(IEnumerable<EmployeeModel> employeeModels)
        {
            var employees = employeeModels.Select(model => new Employees
            {
                UserName = model.Name,
                DeptID = model.DeptID,
            });

            await _employeeRepository.BulkCreateEmployeesAsync(employees);
        }

        public async Task<IEnumerable<EmployeeModel>> GetPagedEmployeesAsync(int pageNumber, int pageSize)
        {
            var employees = await _employeeRepository.GetPagedEmployeesAsync(pageNumber, pageSize);

            return employees.Select(employee => new EmployeeModel
            {
                Name = employee.UserName,
                DeptID = employee.DeptID,
            }).ToList();
        }

        public async Task SaveEmployeePhotoAsync(string employeeId, IFormFile photoFile)
        {
            var employee = await _employeeRepository.GetAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new Exception("Employee not found.");

            // Use the FileUploadService to save the photo file
            var fileName = await _fileService.SaveFileAsync(photoFile, "uploads/employees");

            // Update employee's PhotoFileName
            employee.PhotoPath = fileName;

            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
