
namespace EventApp
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IJwtProvider _jwtProvider;

        public AdminService(IAdminRepository adminRepository, IPasswordEncryptor passwordEncryptor, IJwtProvider jwtProvider)
        {
            _adminRepository = adminRepository;
            _passwordEncryptor = passwordEncryptor;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokenDTO> AuthentifivateAdmin(string email, string password, CancellationToken cancellationToken)
        {
            var admin = await _adminRepository.GetAdminAuthentificationDataAsync(email, cancellationToken);
            if (_passwordEncryptor.VerifyPassword(admin.Password, password))
            {
                return await _jwtProvider.GenerateToken(admin, true, cancellationToken);
            }
            else throw new AuthentificationException();
        }

        public async Task<Guid> CreateAdminAsync(string email, string password, CancellationToken cancellationToken)
        {
            return await _adminRepository.CreateAsync(Guid.NewGuid(), email, _passwordEncryptor.GenerateEncryptedPassword(password), cancellationToken);
        }

        public async Task<Guid> CreateEventAsync(string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string image, CancellationToken cancellationToken)
        {
            return await _adminRepository.CreateEventAsync(Guid.NewGuid(), title, description, eventDateTime, venue, category, maxParticipants, image, cancellationToken);
        }

        public async Task DeleteAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            await _adminRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken)
        {
            await _adminRepository.DeleteEventAsync(eventId, cancellationToken);
        }

        public async Task<AdminModel> GetAdminByIdAsync(Guid adminId, CancellationToken cancellationToken)
        {
            return await _adminRepository.GetAdminByIdAsync(adminId, cancellationToken);
        }

        public async Task UpdateEventAsync(Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string newImage, CancellationToken cancellationToken)
        {
            await _adminRepository.UpdateEventAsync(eventId, newTitle, newDescription, newEventDateTime, newVenue, newCategory, newMaxParticipants, newImage, cancellationToken);
        }
    }
}
