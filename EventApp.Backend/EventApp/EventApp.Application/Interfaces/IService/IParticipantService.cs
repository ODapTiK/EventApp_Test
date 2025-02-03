namespace EventApp
{
    public interface IParticipantService
    {
        public Task<Guid> CreateParticipantAsync(string name, string surname, string email, string password, DateTime birthDate, CancellationToken cancellationToken);
        public Task UpdateParticipantInfoAsync(Guid id, string newName, string newSurname, DateTime newBirthDate, CancellationToken cancellationToken);
        public Task DeleteParticipantAsync(Guid id, CancellationToken cancellationToken);
        public Task SubscribeParticipantToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken);
        public Task UnsubscribeParticipantToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken);
        public Task<ParticipantModel> GetParticipantAsync(Guid id, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetParticipantEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<TokenDTO> AuthentificateParticipant(string email, string password, CancellationToken cancellationToken);
    }
}
