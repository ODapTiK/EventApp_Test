using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace EventApp
{
    [Route("api/[controller]")]
    public class ParticipantController : BaseController
    {
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateParticipantDTO> _createParticipantValidator;
        private readonly IValidator<UpdateParticipantDTO> _updateParticipantValidator;
        private readonly IValidator<AuthParticipantDAO> _authParticipantValidator;

        public ParticipantController(IParticipantService participantService, 
                                        IMapper mapper,
                                        IValidator<CreateParticipantDTO> createParticipantValidator,
                                        IValidator<UpdateParticipantDTO> updateParticipantValidator,
                                        IValidator<AuthParticipantDAO> authParticipantValidator)
        {
            _participantService = participantService;
            _mapper = mapper;
            _createParticipantValidator = createParticipantValidator;
            _updateParticipantValidator = updateParticipantValidator;
            _authParticipantValidator = authParticipantValidator;
        }

        [HttpPut("Auth")]
        public async Task<ActionResult<TokenDTO>> AuthentificateParticipant([FromBody] AuthParticipantDAO authParticipantDAO)
        {
            var validationResult = await _authParticipantValidator.ValidateAsync(authParticipantDAO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var token = await _participantService.AuthentificateParticipant(
                authParticipantDAO.Email,
                authParticipantDAO.Password,
                CancellationToken.None);

            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ParticipantVM>> GetParticipantInfo()
        {
            if (UserId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Participant id must not be empty!");

            var participant = await _participantService.GetParticipantAsync(UserId, CancellationToken.None);

            var participantVM = _mapper.Map<ParticipantVM>(participant);

            return Ok(participantVM);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateParticipant([FromBody] CreateParticipantDTO createParticipantDTO)
        {
            var validationResult = await _createParticipantValidator.ValidateAsync(createParticipantDTO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var participantId = await _participantService.CreateParticipantAsync(
                createParticipantDTO.Name,
                createParticipantDTO.Surname,
                createParticipantDTO.Email,
                createParticipantDTO.Password,
                createParticipantDTO.BirthDate,
                CancellationToken.None);

            return Ok(participantId);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateParticipant([FromBody] UpdateParticipantDTO updateParticipantDTO)
        {
            var validationResult = await _updateParticipantValidator.ValidateAsync(updateParticipantDTO);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _participantService.UpdateParticipantInfoAsync(
                UserId,
                updateParticipantDTO.Name,
                updateParticipantDTO.Surname,
                updateParticipantDTO.BirthDate,
                CancellationToken.None);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteParticipant()
        {
            await _participantService.DeleteParticipantAsync(UserId, CancellationToken.None);

            return NoContent();
        }

        [Authorize]
        [HttpGet("Events/{page}")]
        public async Task<ActionResult<PagedResult<EventVM>>> GetParticipantEvents(int page)
        {
            var pagedEvents = await _participantService.GetParticipantEventsAsync(UserId, page, 10, CancellationToken.None);

            PagedResult<EventVM> events = new()
            {
                Items = _mapper.Map<List<EventVM>>(pagedEvents.Items),
                PageNumber = pagedEvents.PageNumber,
                TotalPages = pagedEvents.TotalPages,
                TotalCount = pagedEvents.TotalCount,
                PageSize = pagedEvents.PageSize
            };

            return Ok(events);
        }

        [Authorize]
        [HttpGet("Events/Subscribe/{eventId}")]
        public async Task<IActionResult> SubscribeParticipantToEvent(Guid eventId)
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id must not be empty!");

            await _participantService.SubscribeParticipantToEventAsync(UserId, eventId, CancellationToken.None);

            return Ok();
        }

        [Authorize]
        [HttpGet("Events/Unsubscribe/{eventId}")]
        public async Task<IActionResult> UnsubscribeParticipantFromEvent(Guid eventId)
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id must not be empty!");

            await _participantService.UnsubscribeParticipantToEventAsync(UserId, eventId, CancellationToken.None);

            return Ok();
        }
    }
}
