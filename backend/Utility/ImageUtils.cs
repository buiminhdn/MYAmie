namespace Utility;
public static class ImageUtils
{
    public static string GetCoverImage(string images)
    {
        return images?.Split(';').FirstOrDefault() ?? string.Empty;
    }
}
