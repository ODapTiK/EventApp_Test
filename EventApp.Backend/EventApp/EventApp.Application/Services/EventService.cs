namespace EventApp
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<EventModel> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken) 
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event Id must not be null or empty.");

            return await _eventRepository.GetByIdAsync(eventId, cancellationToken);
        }

        public async Task<PagedResult<EventModel>> GetEventsListByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetByParamsAsync(title, category, venue, date, pageNumber, pageSize, cancellationToken);
        }

        public async Task<PagedResult<EventModel>> GetAllEventsListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetAllEventsAsync(pageNumber, pageSize, cancellationToken);
        }

    }
}
