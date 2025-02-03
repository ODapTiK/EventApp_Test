namespace EventApp
{
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string name, object key) : base($"Event \"{name}\"({key}) not found!") { }
    }
}
