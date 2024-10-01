﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;




namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var department = await _departmentService.CreateDepartmentAsync(model);
                return Ok(department);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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