namespace EventApp
{
    public interface IAuthenticateParticipantUseCase
    {
        public Task<TokenDTO> Execute(AuthParticipantDAO authParticipantDAO);
    }
}
