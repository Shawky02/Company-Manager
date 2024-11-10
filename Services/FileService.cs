namespace WebApplication1.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
                throw new ArgumentException("Web root path is not configured.");

            // Create folder if it doesn't exist
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentException("Folder path is not valid.");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a unique file name
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            // Ensure the file path is valid
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path is not valid.");

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

    }
}
