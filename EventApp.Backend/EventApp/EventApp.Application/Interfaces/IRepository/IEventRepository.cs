namespace EventApp
{
    public interface IEventRepository
    {
        public Task<EventModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken);

        //public Task<EventModel> GetByTitleAsync(string title, CancellationToken cancellationToken);
        //public Task<List<EventModel>> GetEventsByDateAsync(DateTime date, CancellationToken cancellationToken);
        //public Task<List<EventModel>> GetEventsByVenueAsync(string venue, CancellationToken cancellationToken);
        //public Task<List<EventModel>> GetEventsByCategoryAsync(string category, CancellationToken cancellationToken);
    }
}
