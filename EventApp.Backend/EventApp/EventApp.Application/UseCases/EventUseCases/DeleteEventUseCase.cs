namespace EventApp
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Execute(Guid eventId)
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id can not be empty!");

            var _event = await _eventRepository.FindEventAsync(eventId, CancellationToken.None);

            if (_event == null) 
                throw new EventNotFoundException(nameof(EventModel), eventId);
            else
            {
                await _eventRepository.DeleteAsync(_event, CancellationToken.None);
            }
        }
    }
}
