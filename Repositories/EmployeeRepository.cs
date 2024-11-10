using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Repositories
{
    public class EmployeeRepository : Repository<Employees>, IEmployeeRepository
    {
        private readonly MyContext _context;

        public EmployeeRepository(MyContext context) : base(context)
        {
            _context = context;
        }

        public async Task BulkCreateEmployeesAsync(IEnumerable<Employees> employees)
        {
            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employees>> GetPagedEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _context.Employees
                                 .Skip((pageNumber - 1) * pageSize)  
                                 .Take(pageSize)                     
                                 .ToListAsync();
        }

        public async Task<Employees> GetByIdAsync(string employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        }

    }
}
