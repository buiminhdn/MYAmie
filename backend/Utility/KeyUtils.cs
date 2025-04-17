namespace Utility;
public static class KeyUtils
{
    // Key for verification code for email
    public static string EmailVerificationCode(string email)
    {
        return $"verification:{email}";
    }

    // Key for reset password
    public static string ResetPassword(string email)
    {
        return $"reset-password:{email}";
    }
}
