using WebApplication1.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly MyContext _context;

        public DepartmentService(MyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        {
            var departments = await _context.Departments.ToListAsync();

            var departmentModels = departments.Select(d => new DepartmentModel
            {
                Name = d.Name,
                ID = d.ID
            });

            return departmentModels;
        }

        
        public async Task<DepartmentModel> CreateDepartmentAsync(DepartmentModel model)
        {
            
            if (await DepartmentExistsAsync(model.Name))
            {
                throw new InvalidOperationException("Department already exists.");
            }

            // Map model to entity
            var departmentEntity = new Department
            {
                Name = model.Name.Trim()
            };

            _context.Departments.Add(departmentEntity);
           var result = await _context.SaveChangesAsync();

            if (result == 0)
                throw new Exception("unable to save changes to the database");

            return new DepartmentModel { Name = departmentEntity.Name };
        }

        public async Task<bool> DepartmentExistsAsync(string departmentName)
        {
            return await _context.Departments
                .AnyAsync(d => d.Name.Trim().ToLower() == departmentName.Trim().ToLower());
        }

        //public async Task<bool> DepartmentExistsByIdAsync(int departmentId)
        //{
        //    return await _context.Departments.AnyAsync(d => d.ID == departmentId);
        //}

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var employees = await _context.Employees
                .Where(e => e.DeptID == departmentId)
                .Select(e => new EmployeeModel
                {
                    
                    Name = e.UserName,    
                    DeptID = e.DeptID    
                })
                .ToListAsync();

            return employees;
        }
    }
}
