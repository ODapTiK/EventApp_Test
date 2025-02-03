namespace EventApp
{
    public class LackOfEventSubscriptionException : Exception
    {
        public LackOfEventSubscriptionException(object participant, object _event) : base($"Participant \"{participant}\" not registered for the event \"{_event}\"!") { }
    }
}
