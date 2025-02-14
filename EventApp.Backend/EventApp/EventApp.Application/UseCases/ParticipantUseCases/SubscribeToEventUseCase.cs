namespace EventApp
{
    public class SubscribeToEventUseCase : ISubscribeToEventUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IEventRepository _eventRepository;

        public SubscribeToEventUseCase(IParticipantRepository participantRepository, IEventRepository eventRepository)
        {
            _participantRepository = participantRepository;
            _eventRepository = eventRepository;
        }

        public async Task Execute(Guid userId, Guid eventId)
        {
            if (userId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id can not be empty!");
            else if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id can not be empty!");

            if ((await _participantRepository.FindParticipantAsync(userId, CancellationToken.None) == null)) throw new ParticipantNotFoundException(nameof(ParticipantModel), userId);
            else if((await _eventRepository.FindEventAsync(eventId, CancellationToken.None) == null)) throw new EventNotFoundException(nameof(EventModel), userId);

            if((await _participantRepository.FindSubscriptionAsync(userId, eventId, CancellationToken.None) != null)) throw new EntityAlreadyExistsException(nameof(EventParticipant), userId);

            await _participantRepository.SubscribeToEventAsync(userId, eventId, DateTime.Now.ToUniversalTime(), CancellationToken.None);
        }
    }
}
