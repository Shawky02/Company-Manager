using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApplication1.Entities;

namespace WebApplication1.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

      
    }
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RoleAssignmentModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        
    }

}
