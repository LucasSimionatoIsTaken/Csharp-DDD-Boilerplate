namespace Infrastructure.Extensions;

public static class BCryptExtension
{
    /// <summary>
    /// Generates a hash with bcrypt
    /// </summary>
    /// <param name="password">String to be hashed</param>
    /// <returns>Hash</returns>
    public static string GenerateBCryptHash(this string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verify if a bcrypt hash is valid
    /// </summary>
    /// <param name="hash">Hash to be verified</param>
    /// <param name="password">String to verify</param>
    /// <returns>True if valid</returns>
    public static bool VerifyHash(this string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}