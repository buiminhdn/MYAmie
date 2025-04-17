using Common.Responses;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Utility.Constants;
using WebAPI.Services.Files;

namespace MYAmie.WebAPI.Services.Files;

public class FileService(IWebHostEnvironment environment, ILogger<FileService> logger) : IFileService
{
    private readonly ILogger<FileService> _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;
    private const string UploadsFolderName = "uploads";
    private const int DefaultJpegQuality = 80;

    private static readonly HashSet<string> AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private static readonly HashSet<string> AllowedImageMimeTypes = ["image/jpeg", "image/png", "image/gif"];

    public async Task<ApiResponse<string>> SaveImageAsync(IFormFile image, int maxWidth, int maxHeight)
    {
        try
        {
            var uploadsFolder = EnsureUploadsFolderExists();
            var originalFileName = Path.GetFileName(image.FileName);
            var sanitizedFileName = SanitizeFileName(originalFileName);
            var uniqueFileName = GenerateUniqueFileName(sanitizedFileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = image.OpenReadStream();
            var imageFormat = await GetImageFormatAsync(stream);
            stream.Position = 0;

            using var img = await Image.LoadAsync(stream);
            var resizedImg = ResizeImage(img, maxWidth, maxHeight);

            await SaveImageToFileAsync(resizedImg, imageFormat, filePath);
            return ApiResponse<string>.Success(uniqueFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving image: {FileName}", image.FileName);
            return ApiResponse<string>.Failure(ResponseMessages.ImageSaveFailed);
        }
    }

    public async Task<ApiResponse<IEnumerable<string>>> SaveImagesAsync(IEnumerable<IFormFile> images, int maxWidth, int maxHeight)
    {
        try
        {
            var saveTasks = images.Select(image => SaveImageAsync(image, maxWidth, maxHeight));
            var results = await Task.WhenAll(saveTasks);

            // Check if all operations were successful
            if (results.All(r => r.IsSuccess))
            {
                var fileNames = results.Select(r => r.Data);
                return ApiResponse<IEnumerable<string>>.Success(fileNames);
            }
            else
            {
                // If any failed, return failure with combined error messages
                var errorMessages = results
                    .Where(r => !r.IsSuccess)
                    .Select(r => r.Message)
                    .ToList();
                return ApiResponse<IEnumerable<string>>.Failure(string.Join("; ", errorMessages));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving multiple images");
            return ApiResponse<IEnumerable<string>>.Failure(ResponseMessages.ImageSaveFailed);
        }
    }

    public void DeleteImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;

        try
        {
            var fileName = Path.GetFileName(imagePath);
            var filePath = GetSafeFilePath(fileName);

            if (!File.Exists(filePath)) return;

            _logger.LogInformation("Deleting image: {ImagePath}", imagePath);
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete image: {ImagePath}", imagePath);
            SafeRenameFile(imagePath);
        }
    }

    public void DeleteImages(IEnumerable<string> imagePaths)
    {
        foreach (var path in imagePaths)
            DeleteImage(path);
    }

    public bool IsImageFile(IFormFile file)
    {
        try
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
                return false;

            if (!AllowedImageMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                return false;

            using var stream = file.OpenReadStream();
            return Image.Identify(stream) != null;
        }
        catch
        {
            return false;
        }
    }

    #region Helper Methods

    private string EnsureUploadsFolderExists()
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, UploadsFolderName);
        Directory.CreateDirectory(uploadsFolder);
        return uploadsFolder;
    }

    private static string GenerateUniqueFileName(string fileName)
        => $"{Guid.NewGuid():N}_{fileName}";

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", fileName.Split(invalidChars));
    }

    private static Image ResizeImage(Image image, int maxWidth, int maxHeight)
    {
        if (image.Width <= maxWidth && image.Height <= maxHeight)
            return image;

        var options = new ResizeOptions
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max,
            Sampler = KnownResamplers.Lanczos3
        };

        image.Mutate(x => x.Resize(options));
        return image;
    }

    private static async Task SaveImageToFileAsync(Image image, IImageFormat format, string filePath)
    {
        IImageEncoder encoder = format switch
        {
            JpegFormat => new JpegEncoder { Quality = DefaultJpegQuality },
            PngFormat => new PngEncoder(),
            GifFormat => new GifEncoder(),
            _ => new JpegEncoder { Quality = DefaultJpegQuality }
        };

        await image.SaveAsync(filePath, encoder);
    }

    private static async Task<IImageFormat> GetImageFormatAsync(Stream stream)
    {
        var format = await Image.DetectFormatAsync(stream);
        stream.Position = 0;
        return format ?? throw new InvalidOperationException("Unsupported image format");
    }

    private void SafeRenameFile(string imagePath)
    {
        try
        {
            var fileName = Path.GetFileName(imagePath);
            var filePath = GetSafeFilePath(fileName);

            if (!File.Exists(filePath)) return;

            var directory = Path.GetDirectoryName(filePath);
            var newFileName = $"del_{Guid.NewGuid():N}{Path.GetExtension(filePath)}";
            var newFilePath = Path.Combine(directory!, newFileName);
            File.Move(filePath, newFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rename file: {ImagePath}", imagePath);
        }
    }

    private string GetSafeFilePath(string fileName)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, UploadsFolderName);
        return Path.Combine(uploadsFolder, fileName);
    }

    #endregion
}