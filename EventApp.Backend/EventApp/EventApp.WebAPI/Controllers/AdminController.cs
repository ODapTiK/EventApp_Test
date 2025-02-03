using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp
{
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IValidator<CreateAdminDTO> _createAdminValidator;
        private readonly IValidator<AuthAdminDAO> _authAdminValidator;
        private readonly IValidator<CreateEventDTO> _createEventValidator;
        private readonly IValidator<UpdateEventDTO> _updateEventValidator;

        public AdminController(IAdminService adminService,
                                    IValidator<CreateAdminDTO> createAdminValidator,
                                    IValidator<AuthAdminDAO> authAdminValidator,
                                    IValidator<CreateEventDTO> createEventValidator,
                                    IValidator<UpdateEventDTO> updateEventValidator)
        {
            _adminService = adminService;
            _createAdminValidator = createAdminValidator;
            _createEventValidator = createEventValidator;
            _updateEventValidator = updateEventValidator;
            _authAdminValidator = authAdminValidator;
        }


        [HttpPut("Auth")]
        public async Task<ActionResult<TokenDTO>> AuthAdmin([FromBody] AuthAdminDAO authAdminDAO)
        {
            var validationResult = await _authAdminValidator.ValidateAsync(authAdminDAO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var token = await _adminService.AuthentifivateAdmin(
                authAdminDAO.Email,
                authAdminDAO.Password,
                CancellationToken.None);

            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAdmin([FromBody] CreateAdminDTO createAdminDTO)
        {
            var validationResult = await _createAdminValidator.ValidateAsync(createAdminDTO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var adminId = await _adminService.CreateAdminAsync(
                createAdminDTO.Email,
                createAdminDTO.Password,
                CancellationToken.None);

            return Ok(adminId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin()
        {
            await _adminService.DeleteAdminAsync(UserId, CancellationToken.None);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Event")]
        public async Task<ActionResult<Guid>> CreateEvent([FromBody] CreateEventDTO createEventDTO)
        {
            var validationResult = await _createEventValidator.ValidateAsync(createEventDTO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventId = await _adminService.CreateEventAsync(
                createEventDTO.Title,
                createEventDTO.Description,
                createEventDTO.EventDateTime,
                createEventDTO.Venue,
                createEventDTO.Category,
                createEventDTO.MaxParticipants,
                createEventDTO.Image,
                CancellationToken.None);

            return Ok(eventId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Event")]
        public async Task<IActionResult> UpdateEvent([FromBody]  UpdateEventDTO updateEventDTO)
        {
            var validationResult = await _updateEventValidator.ValidateAsync(updateEventDTO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _adminService.UpdateEventAsync(
                updateEventDTO.Id,
                updateEventDTO.Title,
                updateEventDTO.Description,
                updateEventDTO.EventDateTime,
                updateEventDTO.Venue,
                updateEventDTO.Category,
                updateEventDTO.MaxParticipants,
                updateEventDTO.Image,
                CancellationToken.None);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Event/{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            if (id.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id must not be empty!");

            await _adminService.DeleteEventAsync(id, CancellationToken.None);

            return Ok();
        }
    }
}
