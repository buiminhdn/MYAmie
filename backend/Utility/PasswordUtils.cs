using System.Security.Cryptography;

namespace Utility;
public static class PasswordUtils
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit 
    private const int Iterations = 100000; // Number of iterations
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    /// <summary>
    /// Hash the password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>
    /// Verify the password (Check valid password)
    /// </summary>
    /// <param name="passwordHash"></param>
    /// <param name="inputPassword"></param>
    /// <returns></returns>
    public static bool Verify(string passwordHash, string inputPassword)
    {
        // Split the hash into its parts
        string[] parts = passwordHash.Split(Delimiter);
        byte[] salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}