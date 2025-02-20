namespace EventApp
{
    public interface IAdminRepository
    {
        public Task<Guid> CreateAsync(Guid id, string email, string password, CancellationToken cancellationToken);
        public Task DeleteAsync(AdminModel admin, CancellationToken cancellationToken);
        public Task<AdminModel?> FindAdminAsync(Guid id, CancellationToken cancellationToken);
        public Task<AdminModel?> GetAuthDataAsync(string adminEmail, CancellationToken cancellationToken);
        public Task UpdateAsync(CancellationToken cancellationToken);
    }
}
