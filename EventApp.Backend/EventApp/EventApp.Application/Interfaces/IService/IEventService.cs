namespace EventApp
{
    public interface IEventService
    {
        public Task<EventModel> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetEventsListByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<PagedResult<EventModel>> GetAllEventsListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
