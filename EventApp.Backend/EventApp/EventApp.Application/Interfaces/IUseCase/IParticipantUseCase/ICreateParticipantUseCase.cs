namespace EventApp
{
    public interface ICreateParticipantUseCase
    {
        public Task<Guid> Execute(CreateParticipantDTO createParticipantDTO);
    }
}
