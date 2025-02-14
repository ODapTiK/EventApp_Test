using FluentValidation;

namespace EventApp
{
    public class AuthenticateParticipantUseCase : IAuthenticateParticipantUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IValidator<AuthParticipantDAO> _validator;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticateParticipantUseCase(IParticipantRepository participantRepository, IValidator<AuthParticipantDAO> validator, IPasswordEncryptor passwordEncryptor, IJwtProvider jwtProvider)
        {
            _participantRepository = participantRepository;
            _validator = validator;
            _passwordEncryptor = passwordEncryptor;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokenDTO> Execute(AuthParticipantDAO authParticipantDAO)
        {
            var validationResult = await _validator.ValidateAsync(authParticipantDAO);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var participant = await _participantRepository.GetAuthDataAsync(authParticipantDAO.Email, CancellationToken.None);

            if (participant == null)
            {
                throw new ParticipantNotFoundException(nameof(ParticipantModel), authParticipantDAO.Email);
            }
            else if (_passwordEncryptor.VerifyPassword(participant.Password, authParticipantDAO.Password))
            {
                return await _jwtProvider.GenerateToken(participant, true, CancellationToken.None);
            }
            else throw new AuthenticationException();
        }
    }
}
