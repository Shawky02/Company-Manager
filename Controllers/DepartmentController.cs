using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(Id);
            return Ok(department);
        }

        //[HttpGet]
        //public async Task<IActionResult> CheckDepartmentExists(string departmentName)
        //{
        //    if (await _departmentService.DepartmentExistsAsync(departmentName))
        //    {
        //        return Ok("Department exists.");
        //    }
        //    return NotFound("Department not found.");
        //}

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> GetAllDepartments()
        //{
        //    var departments = await _departmentService.GetAllDepartmentsAsync();
        //    return Ok(departments);
        //}

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //try
            //{
                var department = await _departmentService.CreateDepartmentAsync(model);
                return Ok(department);
            //}
            //catch (InvalidOperationException ex)
            //{
            //    return BadRequest(new { message = ex.Message });
            //}
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _departmentService.GetEmployeesByDepartmentAsync(departmentId);

            if (!employees.Any())
            {
                return NotFound(new { Message = "No employees found for this department." });
            }

            return Ok(employees);  
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartmentById(int departmentId)
        {
            //try
            //{
                await _departmentService.DeleteDepartmentByIdAsync(departmentId);
                return Ok($"Department '{departmentId}' has been deleted.");
            ////}
            ////catch (Exception ex)
            //{
            //    return NotFound(Message);
            //}
        }
    }
}

 //[HttpPost]
        //[Authorize(Roles = "Manager")]
        //public async Task<IActionResult> CreateDepartment([FromBody] Department model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        bool departmentExists = await _context.Departments
        //            .AnyAsync(d => d.Name.Trim().ToLower() == model.Name.Trim().ToLower());

        //        if (departmentExists)
        //        {
        //            return BadRequest(new { message = "A department with this name already exists." });
        //        }

        //        model.Name = model.Name.Trim();
        //        _context.Add(department);
        //        await _context.SaveChangesAsync();
        //        return Ok(department);  
        //    }

        //    return BadRequest(ModelState);  
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        //{

        //    var department = await _context.Departments.FindAsync(departmentId);
        //    if (department == null)
        //    {
        //        return NotFound(new { Message = "Department not found" });
        //    }


        //    var users = await _context.Employees
        //                .Where(e => e.DeptID == departmentId)
        //                .ToListAsync();

        //    if (!users.Any())
        //    {
        //        return NotFound(new { Message = "No users found for this department" });
        //    }


        //    return Ok(users); 
        //}