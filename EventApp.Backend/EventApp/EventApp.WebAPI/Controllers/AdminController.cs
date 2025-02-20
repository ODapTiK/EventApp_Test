using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp
{
    [Route("api/[controller]")]
    public class AdminController : BaseController
    {
        private readonly ICreateAdminUseCase _createAdminUseCase;
        private readonly IDeleteAdminUseCase _deleteAdminUseCase;
        private readonly IAuthenticateAdminUseCase _authenticateAdminUseCase;

        public AdminController(ICreateAdminUseCase createAdminUseCase, IDeleteAdminUseCase deleteAdminUseCase, IAuthenticateAdminUseCase authenticateAdminUseCase)
        {
            _createAdminUseCase = createAdminUseCase;
            _deleteAdminUseCase = deleteAdminUseCase;
            _authenticateAdminUseCase = authenticateAdminUseCase;
        }

        [HttpPut("Auth")]
        public async Task<ActionResult<TokenDTO>> AuthAdmin([FromBody] AuthAdminDAO authAdminDAO)
        {
            var token = await _authenticateAdminUseCase.Execute(authAdminDAO);

            return Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAdmin([FromBody] CreateAdminDTO createAdminDTO)
        {
            var adminId = await _createAdminUseCase.Execute(createAdminDTO);

            return Ok(adminId);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin()
        {
            await _deleteAdminUseCase.Execute(UserId);

            return Ok();
        }
    }
}
