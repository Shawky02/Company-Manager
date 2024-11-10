using System.ComponentModel.DataAnnotations;
using WebApplication1.Extensions;

namespace WebApplication1.Models
{
    public class PhotoModel
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        public required IFormFile PhotoFile { get; set; }
    }
}
