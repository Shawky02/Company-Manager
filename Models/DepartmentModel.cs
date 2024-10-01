using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class DepartmentModel
    {
        [Required]
        public string Name { get; set; }
        public int ID { get; set; }
    }
    public class EmployeeModel
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? DeptID { get; set; }
    }

}
