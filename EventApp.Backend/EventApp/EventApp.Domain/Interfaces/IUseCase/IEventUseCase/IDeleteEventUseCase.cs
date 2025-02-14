namespace EventApp
{
    public interface IDeleteEventUseCase
    {
        public Task Execute(Guid id);
    }
}
