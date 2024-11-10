using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MyContext context) : base(context)
        {
        }
    }
}
