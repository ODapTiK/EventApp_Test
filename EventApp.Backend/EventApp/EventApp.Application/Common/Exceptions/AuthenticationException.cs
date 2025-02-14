namespace EventApp
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base($"Access denied!") { }
    }
}
