namespace EventApp
{
    public interface IUnsubscribeFromEventUseCase
    {
        public Task Execute(Guid userId, Guid eventId);
    }
}
