namespace WebApplication1.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
    }
}
