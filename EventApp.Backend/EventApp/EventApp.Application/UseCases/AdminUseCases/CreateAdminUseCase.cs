using FluentValidation;

namespace EventApp
{
    public class CreateAdminUseCase : ICreateAdminUseCase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IValidator<CreateAdminDTO> _validator;
        private readonly IPasswordEncryptor _passwordEncryptor;

        public CreateAdminUseCase(IAdminRepository adminRepository, IValidator<CreateAdminDTO> validator, IPasswordEncryptor passwordEncryptor)
        {
            _adminRepository = adminRepository;
            _validator = validator;
            _passwordEncryptor = passwordEncryptor;
        }

        public async Task<Guid> Execute(CreateAdminDTO createAdminDTO)
        {
            var validationResult = await _validator.ValidateAsync(createAdminDTO);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }
            else if((await _adminRepository.GetAuthDataAsync(createAdminDTO.Email, CancellationToken.None)) != null)
            {
                throw new EntityAlreadyExistsException(nameof(AdminModel), createAdminDTO.Email);
            }
            else
            {
                return await _adminRepository.CreateAsync(
                    Guid.NewGuid(),
                    createAdminDTO.Email,
                    _passwordEncryptor.GenerateEncryptedPassword(createAdminDTO.Password),
                    CancellationToken.None);
            }
        }
    }
}
