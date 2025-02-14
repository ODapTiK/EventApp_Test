namespace EventApp
{
    public interface IUpdateEventUseCase
    {
        public Task Execute(UpdateEventDTO updateEventDTO);
    }
}
