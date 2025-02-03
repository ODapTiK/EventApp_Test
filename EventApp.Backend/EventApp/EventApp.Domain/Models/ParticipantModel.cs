namespace EventApp
{
    public class ParticipantModel : User
    {
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<EventParticipant> Events { get; set; } = new List<EventParticipant>();
    }
}
