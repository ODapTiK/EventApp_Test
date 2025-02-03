namespace EventApp
{
    public class AuthentificationException : Exception
    {
        public AuthentificationException() : base($"Access denied!") { }
    }
}
