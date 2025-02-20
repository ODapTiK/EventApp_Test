namespace EventApp
{
    public interface ISubscribeToEventUseCase
    {
        public Task Execute(Guid userId, Guid eventId);
    }
}
