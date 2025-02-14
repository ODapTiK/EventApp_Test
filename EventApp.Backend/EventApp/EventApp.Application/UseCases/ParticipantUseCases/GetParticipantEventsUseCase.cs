namespace EventApp
{
    public class GetParticipantEventsUseCase : IGetParticipantEventsUseCase
    {
        private readonly IParticipantRepository _participantRepository;

        public GetParticipantEventsUseCase(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<PagedResult<EventVM>> Execute(Guid userId, int pageNumber, int pageSize)
        {
            if (userId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id can not be empty!");

            if((await _participantRepository.FindParticipantAsync(userId, CancellationToken.None)) == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), userId);

            return await _participantRepository.GetEventsAsync(userId, pageNumber, pageSize, CancellationToken.None);
        }
    }
}
