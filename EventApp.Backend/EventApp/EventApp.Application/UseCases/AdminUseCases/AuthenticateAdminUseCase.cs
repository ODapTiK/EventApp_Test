using FluentValidation;

namespace EventApp
{
    public class AuthenticateAdminUseCase : IAuthenticateAdminUseCase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IValidator<AuthAdminDAO> _validator;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticateAdminUseCase(IAdminRepository adminRepository, IValidator<AuthAdminDAO> validator, IPasswordEncryptor passwordEncryptor, IJwtProvider jwtProvider)
        {
            _adminRepository = adminRepository;
            _validator = validator;
            _passwordEncryptor = passwordEncryptor;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokenDTO> Execute(AuthAdminDAO authAdminDAO)
        {
            var validationResult = await _validator.ValidateAsync(authAdminDAO);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var admin = await _adminRepository.GetAuthDataAsync(authAdminDAO.Email, CancellationToken.None);

            if (admin == null)
            {
                throw new AdminNotFoundException(nameof(AdminModel), authAdminDAO.Email);
            }
            else if (_passwordEncryptor.VerifyPassword(admin.Password, authAdminDAO.Password))
            {
                return await _jwtProvider.GenerateToken(admin, true, CancellationToken.None);
            }
            else throw new AuthenticationException();
        }
    }
}
