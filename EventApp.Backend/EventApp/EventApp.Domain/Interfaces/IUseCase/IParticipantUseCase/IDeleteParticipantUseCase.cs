namespace EventApp
{
    public interface IDeleteParticipantUseCase
    {
        public Task Execute(Guid userId);
    }
}
