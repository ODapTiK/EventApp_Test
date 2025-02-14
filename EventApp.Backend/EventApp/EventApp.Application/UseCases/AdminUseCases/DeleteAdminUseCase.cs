namespace EventApp
{
    public class DeleteAdminUseCase : IDeleteAdminUseCase
    {
        private readonly IAdminRepository _adminRepository;

        public DeleteAdminUseCase(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task Execute(Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new FluentValidation.ValidationException("Admin id can not be empty!");

            var admin = await _adminRepository.FindAdminAsync(id, CancellationToken.None);

            if(admin == null)
                throw new AdminNotFoundException(nameof(AdminModel), id);
            else 
                await _adminRepository.DeleteAsync(admin, CancellationToken.None);
        }
    }
}
