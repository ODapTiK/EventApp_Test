namespace EventApp
{
    public interface ICreateAdminUseCase
    {
        public Task<Guid> Execute(CreateAdminDTO createAdminDTO);
    }
}
