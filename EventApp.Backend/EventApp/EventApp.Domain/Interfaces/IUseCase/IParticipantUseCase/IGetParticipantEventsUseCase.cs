namespace EventApp
{
    public interface IGetParticipantEventsUseCase
    {
        public Task<PagedResult<EventVM>> Execute(Guid userId, int pageNumber, int pageSize);
    }
}
