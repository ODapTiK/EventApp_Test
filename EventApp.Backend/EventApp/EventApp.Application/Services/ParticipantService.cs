namespace EventApp
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IJwtProvider _jwtProvider;

        public ParticipantService(IParticipantRepository participantRepository, IPasswordEncryptor passwordEncryptor, IJwtProvider jwtProvider)
        {
            _participantRepository = participantRepository;
            _passwordEncryptor = passwordEncryptor;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokenDTO> AuthentificateParticipant(string email, string password, CancellationToken cancellationToken)
        {
            var participant = await _participantRepository.GetAuthentificationInfoAsync(email, cancellationToken);
            if (_passwordEncryptor.VerifyPassword(participant.Password, password))
            {
                return await _jwtProvider.GenerateToken(participant, true, cancellationToken);
            }
            else throw new AuthentificationException();
        }

        public async Task<Guid> CreateParticipantAsync(string name, string surname, string email, string password, DateTime birthDate, CancellationToken cancellationToken)
        {
            return await _participantRepository.CreateAsync(Guid.NewGuid(), name, surname, email, _passwordEncryptor.GenerateEncryptedPassword(password), birthDate, cancellationToken);
        }

        public async Task DeleteParticipantAsync(Guid id, CancellationToken cancellationToken)
        {
            await _participantRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<ParticipantModel> GetParticipantAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _participantRepository.GetAsync(id, cancellationToken);
        }

        public async Task<PagedResult<EventModel>> GetParticipantEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _participantRepository.GetEventsAsync(id, pageNumber, pageSize, cancellationToken);
        }

        public async Task SubscribeParticipantToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken)
        {
            await _participantRepository.SubscribeToEventAsync(id, eventId, cancellationToken);
        }

        public async Task UnsubscribeParticipantToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken)
        {
            await _participantRepository.UnsubscribeToEventAsync(id, eventId, cancellationToken);
        }

        public async Task UpdateParticipantInfoAsync(Guid id, string newName, string newSurname, DateTime newBirthDate, CancellationToken cancellationToken)
        {
            await _participantRepository.UpdateAsync(id, newName, newSurname, newBirthDate, cancellationToken);
        }
    }
}
