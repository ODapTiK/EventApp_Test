using FluentValidation;

namespace EventApp
{
    public class CreateParticipantUseCase : ICreateParticipantUseCase
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IValidator<CreateParticipantDTO> _validator;
        private readonly IPasswordEncryptor _passwordEncryptor;

        public CreateParticipantUseCase(IParticipantRepository participantRepository, IValidator<CreateParticipantDTO> validator, IPasswordEncryptor passwordEncryptor)
        {
            _participantRepository = participantRepository;
            _validator = validator;
            _passwordEncryptor = passwordEncryptor;
        }

        public async Task<Guid> Execute(CreateParticipantDTO createParticipantDTO)
        {
            var validationResult = await _validator.ValidateAsync(createParticipantDTO);
            if(!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }
            else if((await _participantRepository.GetAuthDataAsync(createParticipantDTO.Email, CancellationToken.None)) != null)
            {
                throw new EntityAlreadyExistsException(nameof(ParticipantModel), createParticipantDTO.Email);
            }
            else
            {
                return await _participantRepository.CreateAsync(
                    Guid.NewGuid(),
                    createParticipantDTO.Name,
                    createParticipantDTO.Surname,
                    createParticipantDTO.Email,
                    _passwordEncryptor.GenerateEncryptedPassword(createParticipantDTO.Password),
                    createParticipantDTO.BirthDate.ToUniversalTime(),
                    CancellationToken.None);
            }
        }
    }
}
