namespace EventApp
{
    public class UnsubscribeFromEventUseCase : IUnsubscribeFromEventUseCase
    {
        private readonly IParticipantRepository _participantRepository;

        public UnsubscribeFromEventUseCase(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task Execute(Guid userId, Guid eventId)
        {
            if (userId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id can not be empty!");
            else if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id can not be empty!");

            var subscription = await _participantRepository.FindSubscriptionAsync(userId, eventId, CancellationToken.None);

            if(subscription == null) throw new LackOfEventSubscriptionException(userId, eventId);   

            await _participantRepository.UnsubscribeToEventAsync(subscription, CancellationToken.None);
        }
    }
}
