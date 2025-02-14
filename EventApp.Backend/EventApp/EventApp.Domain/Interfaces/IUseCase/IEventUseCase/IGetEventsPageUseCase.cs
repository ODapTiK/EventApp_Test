namespace EventApp
{
    public interface IGetEventsPageUseCase
    {
        public Task<PagedResult<EventVM>> Execute(int pageNumber, int pageSize);
    }
}
