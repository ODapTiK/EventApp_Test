using Microsoft.AspNetCore.Mvc;

namespace EventApp
{
    [Route("api/Token")]
    public class TokenController : BaseController
    {
        private readonly IJwtProvider _jwtProvider;

        public TokenController(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }
        [HttpPut("Refresh")]
        public async Task<ActionResult<TokenDTO>> Refresh([FromBody] TokenDTO tokenDto)
        {
            var tokenDtoToReturn = await _jwtProvider.RefreshToken(tokenDto);

            return Ok(tokenDtoToReturn);
        }
    }
}
