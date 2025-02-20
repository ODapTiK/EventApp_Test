namespace EventApp
{
    public interface IGetEventByIdUseCase
    {
        public Task<EventVM> Execute(Guid eventId);
    }
}
