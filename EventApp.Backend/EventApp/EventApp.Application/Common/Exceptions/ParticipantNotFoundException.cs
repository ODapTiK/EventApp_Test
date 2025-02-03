namespace EventApp
{
    public class ParticipantNotFoundException : Exception
    {
        public ParticipantNotFoundException(string name, object key) : base($"Participant \"{name}\"({key}) not found!") { }
    }
}
