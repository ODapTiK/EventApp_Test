namespace EventApp
{
    public class GetEventByIdUseCase : IGetEventByIdUseCase
    {
        private readonly IEventRepository _eventRepository;

        public GetEventByIdUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<EventVM> Execute(Guid eventId)
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id can not be empty!");
            if ((await _eventRepository.FindEventAsync(eventId, CancellationToken.None)) == null) throw new EventNotFoundException(nameof(EventModel), eventId);
            
            return await _eventRepository.GetByIdAsync(eventId, CancellationToken.None);
        }
    }
}
