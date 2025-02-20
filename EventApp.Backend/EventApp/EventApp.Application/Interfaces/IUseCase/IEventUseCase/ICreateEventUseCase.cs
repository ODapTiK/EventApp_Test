namespace EventApp
{
    public interface ICreateEventUseCase
    {
        public Task<Guid> Execute(CreateEventDTO createEventDTO);
    }
}
