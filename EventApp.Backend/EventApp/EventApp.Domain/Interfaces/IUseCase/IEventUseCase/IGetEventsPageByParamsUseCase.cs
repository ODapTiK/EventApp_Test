namespace EventApp
{
    public interface IGetEventsPageByParamsUseCase
    {
        public Task<PagedResult<EventVM>> Execute(EventFilterParams eventFilterParams, int pageSize);
    }
}
