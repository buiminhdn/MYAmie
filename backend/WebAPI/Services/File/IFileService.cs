using Common.Responses;

namespace WebAPI.Services.Files;

public interface IFileService
{
    Task<ApiResponse<string>> SaveImageAsync(IFormFile image, int maxWidth, int maxHeight);
    Task<ApiResponse<IEnumerable<string>>> SaveImagesAsync(IEnumerable<IFormFile> images, int maxWidth, int maxHeight);
    void DeleteImage(string imagePath);
    void DeleteImages(IEnumerable<string> imagePaths);
    bool IsImageFile(IFormFile file);
}
