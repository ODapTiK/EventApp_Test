namespace EventApp
{
    public interface IDeleteAdminUseCase
    {
        public Task Execute(Guid id);
    }
}
