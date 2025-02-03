namespace EventApp
{
    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException(string name, object key) : base($"Admin \"{name}\"({key}) not found!") { }
    }
}
