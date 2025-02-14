namespace EventApp
{
    public interface IParticipantRepository
    {
        public Task<Guid> CreateAsync(Guid id, string name, string surname, string email, string password, DateTime BirthDate, CancellationToken cancellationToken);
        public Task UpdateAsync(ParticipantModel participant, string newName, string newSurname, DateTime newBirthDate, CancellationToken cancellationToken);
        public Task DeleteAsync(ParticipantModel participant, CancellationToken cancellationToken);
        public Task SubscribeToEventAsync(Guid id, Guid eventId, DateTime registrationDateTime, CancellationToken cancellationToken);
        public Task UnsubscribeToEventAsync(EventParticipant subscription, CancellationToken cancellationToken);
        public Task<ParticipantModel?> GetAsync(Guid id, CancellationToken cancellationToken);
        public Task<ParticipantModel?> FindParticipantAsync(Guid id, CancellationToken cancellationToken);
        public Task<PagedResult<EventVM>> GetEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<ParticipantModel?> GetAuthDataAsync(string email, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(ParticipantModel participant, string refreshToken, DateTime RefreshTokenExpiryTime, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(ParticipantModel participant, string refreshToken, CancellationToken cancellationToken);
        public Task<EventParticipant?> FindSubscriptionAsync(Guid participantId, Guid eventId, CancellationToken cancellationToken);
    }
}
