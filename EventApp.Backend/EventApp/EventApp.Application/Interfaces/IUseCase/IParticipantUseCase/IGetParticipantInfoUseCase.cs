namespace EventApp
{
    public interface IGetParticipantInfoUseCase
    {
        public Task<ParticipantVM> Execute(Guid userId);
    }
}
