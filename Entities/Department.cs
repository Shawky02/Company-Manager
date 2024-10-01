using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Entities
{
    public class Department
    {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        
    }
}
