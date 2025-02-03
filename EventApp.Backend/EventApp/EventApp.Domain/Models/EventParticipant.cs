namespace EventApp
{
    public class EventParticipant
    {
        public Guid EventId { get; set; }
        public EventModel Event { get; set; } = null!;

        public Guid ParticipantId { get; set; }
        public ParticipantModel Participant { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }
    }
}
