namespace EventApp
{
    public class PasswordEncryptor : IPasswordEncryptor
    {
        public string GenerateEncryptedPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool VerifyPassword(string encryptedPassword, string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, encryptedPassword);
        }
    }
}
