namespace EventApp
{
    public interface IUpdateParticipantUseCase
    {
        public Task Execute(UpdateParticipantDTO updateParticipantDTO, Guid userId);
    }
}
