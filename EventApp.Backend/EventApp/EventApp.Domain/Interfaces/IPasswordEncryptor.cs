namespace EventApp
{
    public interface IPasswordEncryptor
    {
        public string GenerateEncryptedPassword(string password);
        public bool VerifyPassword(string encryptedPAssword, string password);
    }
}
