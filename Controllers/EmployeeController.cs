using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly MyContext _context;
        private readonly UserManager<Employees> _userManager;
        private readonly SignInManager<Employees> _signInManager;

        public EmployeeController(IEmployeeService employeeService,
            MyContext context,
            UserManager<Employees> userManager,
            SignInManager<Employees> signInManager)
        {
            _employeeService = employeeService;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.RegisterEmployeeAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully!" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        // Register a new user
        //[HttpPost]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var user = new Employees { UserName = model.Username};
        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(user, "Employee");  
        //        return Ok(new { Message = "User registered successfully!" });
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return BadRequest(ModelState);
        //}

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.LoginAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful!" });
            }

            return Unauthorized(new { Message = "Invalid username or password." });
        }

        // User Login
        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

        //    if (result.Succeeded)
        //    {
        //        return Ok(new { Message = "Login successful!" });
        //    }

        //    return Unauthorized(new { Message = "Invalid username or password." });
        //}

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        {
            var roleResult = await _employeeService.AssignRoleAsync(model);

            if (roleResult.Succeeded)
            {
                return Ok(new { Message = $"Role {model.Role} assigned to user {model.Username} successfully" });
            }

            return BadRequest(roleResult.Errors);
        }

        //[HttpPost]
        //public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user == null)
        //        return NotFound("User not found");

        //    var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
        //    if (roleResult.Succeeded)
        //        return Ok(new { Message = $"Role {model.Role} assigned to user {model.Username} successfully" });

        //    return BadRequest(roleResult.Errors);
        //}

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AssignEmployeeToDepartment(int departmentId, string employeeId)
        {
            var result = await _employeeService.AssignEmployeeToDepartmentAsync(departmentId, employeeId);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Employee assigned to department successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _employeeService.LogoutAsync();
            return Ok(new { Message = "Logout successful!" });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> BulkCreateEmployees([FromBody] List<EmployeeModel> employeeModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //try
            //{
                await _employeeService.BulkCreateEmployeesAsync(employeeModels);
                return Ok(new { message = "Employees created successfully." });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { message = ex.Message });
            //}

            //[Authorize(Roles = "Manager")]  
            //[HttpPost]
            //public async Task<IActionResult> AssignEmployeeToDepartment(int DepartmentId, string ID)
            //{

            //    //var currentUser = await _userManager.GetUserAsync(User);


            //    //var isManager = await _userManager.IsInRoleAsync(currentUser, "Manager");
            //    //if (!isManager)
            //    //{
            //    //    return Forbid();
            //    //}


            //    var user = await _userManager.FindByIdAsync(ID);
            //    if (user == null)
            //    {
            //        return NotFound(new { Message = "Employee not found" });
            //    }


            //    var department = await _context.Departments.FindAsync(DepartmentId);
            //    if (department == null)
            //    {
            //        return BadRequest(new { Message = "The specified department does not exist" });
            //    }

            //    user.DeptID = DepartmentId;
            //    _context.Employees.Update(user);
            //    await _context.SaveChangesAsync();

            //    return Ok(new { Message = $"Employee {user.UserName} assigned to department {department.Name} successfully" });
            //}


        }

        [HttpGet]
        public async Task<IActionResult> GetPagedEmployees(int pageNumber = 1, int pageSize = 2)
        {
            //try
            //{
                var employees = await _employeeService.GetPagedEmployeesAsync(pageNumber, pageSize);
                return Ok(employees);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { message = ex.Message });
            //}
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPhoto([FromForm] PhotoModel model)
        {
            if (model.PhotoFile == null || model.PhotoFile.Length == 0)
            {
                return BadRequest("Invalid photo file.");
            }

            //try
            //{
                await _employeeService.SaveEmployeePhotoAsync(model.Id, model.PhotoFile);
                return Ok(new { message = "Photo uploaded successfully." });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { message = ex.Message });
            //}
        }
    }
}
