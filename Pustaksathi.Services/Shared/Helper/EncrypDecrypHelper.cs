using System.Security.Cryptography;

namespace Pustaksathi.Services.Helper
{
    public class EncrypDecrypHelper
    {
        private static readonly int SalltSize = 15;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 1000;

        public static string Encrypt(string password)
        {
            byte[] salt;
            RandomNumberGenerator.Fill(salt = new byte[SalltSize]);

            using var pdkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pdkdf2.GetBytes(HashSize);
            byte[] hashBytes = new byte[SalltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SalltSize);
            Array.Copy(hash, 0, hashBytes, SalltSize, HashSize);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string PasswordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(PasswordHash);
            byte[] salt = new byte[SalltSize];
            Array.Copy(hashBytes, 0, salt, 0, SalltSize);

            using var pdkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pdkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SalltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
