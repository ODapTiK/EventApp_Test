namespace EventApp
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
