namespace EventApp
{
    public interface IAdminService
    {
        public Task<Guid> CreateEventAsync
            (string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string imageBase64, CancellationToken cancellationToken);
        public Task UpdateEventAsync
            (Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string imageBase64, CancellationToken cancellationToken);
        public Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken);
        public Task<Guid> CreateAdminAsync(string email, string password, CancellationToken cancellationToken);
        public Task<AdminModel> GetAdminByIdAsync (Guid adminId, CancellationToken cancellationToken);
        public Task DeleteAdminAsync(Guid id, CancellationToken cancellationToken);
        public Task<TokenDTO> AuthentifivateAdmin(string email, string password, CancellationToken cancellationToken);
    }
}
