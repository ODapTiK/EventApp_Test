namespace EventApp
{
    public class GetEventsPageUseCase : IGetEventsPageUseCase
    {
        private readonly IEventRepository _eventRepository;

        public GetEventsPageUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<PagedResult<EventVM>> Execute(int pageNumber, int pageSize)
        {
            return await _eventRepository.GetPageAsync(pageNumber, pageSize, CancellationToken.None);
        }
    }
}
