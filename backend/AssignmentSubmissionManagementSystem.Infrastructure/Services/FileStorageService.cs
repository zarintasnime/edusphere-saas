using AssignmentSubmissionManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
    {
        var sanitizedFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(sanitizedFileName))
        {
            sanitizedFileName = $"file_{Guid.NewGuid():N}.dat";
        }

        var rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadFolder = Path.Combine(rootPath, "uploads", "submissions");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, sanitizedFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        return Path.Combine("uploads", "submissions", sanitizedFileName).Replace("\\", "/");
    }
}