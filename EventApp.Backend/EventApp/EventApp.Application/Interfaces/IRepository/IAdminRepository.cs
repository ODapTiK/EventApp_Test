namespace EventApp
{
    public interface IAdminRepository
    {
        public Task<Guid> CreateEventAsync
            (Guid id, string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string image, CancellationToken cancellationToken);
        public Task UpdateEventAsync
            (Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string newImage, CancellationToken cancellationToken);
        public Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken);
        public Task<Guid> CreateAsync(Guid id, string email, string password, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<AdminModel> GetAdminByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<AdminModel> GetAdminAuthentificationDataAsync(string adminEmail, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(Guid id, string refreshToken, DateTime RefreshTokenExpiryTime, CancellationToken cancellationToken);
        public Task UpdateRefreshTokenAsync(Guid id, string refreshToken, CancellationToken cancellationToken);
    }
}
