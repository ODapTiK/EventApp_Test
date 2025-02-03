namespace EventApp
{
    public class ExcessNumberOfParticipantsException : Exception
    {
        public ExcessNumberOfParticipantsException(EventModel _event) : base($"Event \"{_event.Title}\"({_event.Id}) has limit of participants, max = {_event.MaxParticipants}!") { }
    }
}
