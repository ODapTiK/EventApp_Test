namespace EventApp
{
    public interface IEventRepository
    {
        public Task<Guid> CreateAsync
            (Guid id, string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string image, CancellationToken cancellationToken);
        public Task UpdateAsync
            (EventModel _event, Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string newImage, CancellationToken cancellationToken);
        public Task DeleteAsync(EventModel _event, CancellationToken cancellationToken);
        public Task<EventModel?> FindEventAsync(Guid id, CancellationToken cancellationToken);
        public Task<EventVM> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PagedResult<EventVM>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<PagedResult<EventVM>> GetPageByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken);

    }
}
