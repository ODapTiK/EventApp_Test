namespace EventApp
{
    public interface IParticipantRepository
    {
        public Task<Guid> CreateAsync(Guid id, string name, string surname, string email, string password, DateTime BirthDate, CancellationToken cancellationToken);
        public Task UpdateAsync(Guid id, string newName, string newSurname, DateTime newBirthDate, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task SubscribeToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken);
        public Task UnsubscribeToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken);
        public Task<ParticipantModel> GetAsync(Guid id, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<ParticipantModel> GetAuthentificationInfoAsync(string email, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(Guid id, string refreshToken, DateTime RefreshTokenExpiryTime, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(Guid id, string refreshToken, CancellationToken cancellationToken);
    }
}
