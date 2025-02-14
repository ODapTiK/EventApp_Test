
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IEventAppDbContext _dbContext;

        public AdminRepository(IEventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Guid id, string email, string password, CancellationToken cancellationToken)
        {
                var admin = new AdminModel
                {
                    Id = id,
                    Email = email,
                    Password = password
                };

                await _dbContext.Admins.AddAsync(admin, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return admin.Id;
        }

        public async Task DeleteAsync(AdminModel admin, CancellationToken cancellationToken)
        {
            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<AdminModel?> GetAuthDataAsync(string adminEmail, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email.Equals(adminEmail), cancellationToken);

            return admin;
        }

        public async Task<AdminModel?> FindAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FindAsync([id], cancellationToken);

            return admin;
        }

        public async Task UpdateRefreshTokenAsync(AdminModel admin, string refreshToken, DateTime refreshTokenExpiryTime, CancellationToken cancellationToken)
        {
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(AdminModel admin, string refreshToken, CancellationToken cancellationToken)
        {
            admin.RefreshToken = refreshToken;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
