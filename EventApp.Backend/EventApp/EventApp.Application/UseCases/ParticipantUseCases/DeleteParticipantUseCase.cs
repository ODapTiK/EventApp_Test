namespace EventApp
{
    public class DeleteParticipantUseCase : IDeleteParticipantUseCase
    {
        private readonly IParticipantRepository _participantRepository;

        public DeleteParticipantUseCase(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task Execute(Guid userId)
        {
            if (userId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id can not be empty!");

            var participant = await _participantRepository.FindParticipantAsync(userId, CancellationToken.None);

            if(participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), userId);

            await _participantRepository.DeleteAsync(participant, CancellationToken.None);
        }
    }
}
