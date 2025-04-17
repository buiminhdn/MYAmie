namespace Utility;
public static class NumberUtils
{
    public static int GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(1000, 9999); // Generates a 4-digit code
    }
}
