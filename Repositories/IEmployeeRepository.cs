using WebApplication1.Entities;

namespace WebApplication1.Repositories
{
    public interface IEmployeeRepository : IRepository<Employees>
    {
        Task BulkCreateEmployeesAsync(IEnumerable<Employees> employees);
        Task<IEnumerable<Employees>> GetPagedEmployeesAsync(int pageNumber, int pageSize);
        Task<Employees> GetByIdAsync(string employeeId);
    }
}
