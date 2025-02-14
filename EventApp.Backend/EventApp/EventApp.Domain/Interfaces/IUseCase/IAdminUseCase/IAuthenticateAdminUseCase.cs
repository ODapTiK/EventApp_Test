namespace EventApp
{
    public interface IAuthenticateAdminUseCase
    {
        public Task<TokenDTO> Execute(AuthAdminDAO authAdminDAO);
    }
}
