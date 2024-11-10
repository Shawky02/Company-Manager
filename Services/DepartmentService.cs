using WebApplication1.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Repositories;


namespace WebApplication1.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly MyContext _context;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(MyContext context, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
        }
        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        //public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        //{
        //    var departments = await _context.Departments.ToListAsync();

        //    var departmentModels = departments.Select(d => new DepartmentModel
        //    {
        //        Name = d.Name,
        //        ID = d.ID
        //    });

        //    return departmentModels;
        //}


        public async Task<DepartmentModel> CreateDepartmentAsync(DepartmentModel model)
        {
            if (await DepartmentExistsAsync(model.Name))
            {
                throw new InvalidOperationException("Department already exists.");
            }

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
            return await _departmentRepository.ExistsAsync(d => d.Name.Trim().ToLower() == departmentName);
        }
        //public async Task<bool> DepartmentExistsAsync(string departmentName)
        //{
        //    return await _context.Departments
        //        .AnyAsync(d => d.Name.Trim().ToLower() == departmentName.Trim().ToLower());
        //}

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
        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetAsync(d => d.ID == id);
        }
        public async Task DeleteDepartmentByIdAsync(int departmentId)
        {
            await _departmentRepository.DeleteAsync(d => d.ID == departmentId);
        }
    }
}
