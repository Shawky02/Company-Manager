using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entities
{
    public class Employees:IdentityUser
    {
        public int Salary { get; set; }
        public string PhotoPath { get; set; }

        public int? DeptID { get; set; }

        [ForeignKey(nameof(DeptID))]
        public Department Department { get; set; }




    }
}
