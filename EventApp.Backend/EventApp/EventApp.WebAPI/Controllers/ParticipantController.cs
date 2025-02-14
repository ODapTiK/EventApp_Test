using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace EventApp
{
    [Route("api/[controller]")]
    public class ParticipantController : BaseController
    {
        private readonly ICreateParticipantUseCase _createParticipantUseCase;
        private readonly IUpdateParticipantUseCase _updateParticipantUseCase;
        private readonly IDeleteParticipantUseCase _deleteParticipantUseCase;
        private readonly IAuthenticateParticipantUseCase _authenticateParticipantUseCase;   
        private readonly IGetParticipantInfoUseCase _getParticipantInfoUseCase;
        private readonly IGetParticipantEventsUseCase _getParticipantEventsUseCase;
        private readonly ISubscribeToEventUseCase _subscribeToEventUseCase;
        private readonly IUnsubscribeFromEventUseCase _unsubscribeFromEventUseCase;

        public ParticipantController(ICreateParticipantUseCase createParticipantUseCase, 
                                     IUpdateParticipantUseCase updateParticipantUseCase, 
                                     IDeleteParticipantUseCase deleteParticipantUseCase, 
                                     IAuthenticateParticipantUseCase authenticateParticipantUseCase, 
                                     IGetParticipantInfoUseCase getParticipantInfoUseCase, 
                                     IGetParticipantEventsUseCase getParticipantEventsUseCase, 
                                     ISubscribeToEventUseCase subscribeToEventUseCase, 
                                     IUnsubscribeFromEventUseCase unsubscribeFromEventUseCase)
        {
            _createParticipantUseCase = createParticipantUseCase;
            _updateParticipantUseCase = updateParticipantUseCase;
            _deleteParticipantUseCase = deleteParticipantUseCase;
            _authenticateParticipantUseCase = authenticateParticipantUseCase;
            _getParticipantInfoUseCase = getParticipantInfoUseCase;
            _getParticipantEventsUseCase = getParticipantEventsUseCase;
            _subscribeToEventUseCase = subscribeToEventUseCase;
            _unsubscribeFromEventUseCase = unsubscribeFromEventUseCase;
        }

        [HttpPut("Auth")]
        public async Task<ActionResult<TokenDTO>> AuthentificateParticipant([FromBody] AuthParticipantDAO authParticipantDAO)
        {
            var token = await _authenticateParticipantUseCase.Execute(authParticipantDAO);

            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ParticipantVM>> GetParticipantInfo()
        {
            var participantVM = await _getParticipantInfoUseCase.Execute(UserId);

            return Ok(participantVM);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateParticipant([FromBody] CreateParticipantDTO createParticipantDTO)
        {
            var participantId = await _createParticipantUseCase.Execute(createParticipantDTO);

            return Ok(participantId);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateParticipant([FromBody] UpdateParticipantDTO updateParticipantDTO)
        {
            await _updateParticipantUseCase.Execute(updateParticipantDTO, UserId);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteParticipant()
        {
            await _deleteParticipantUseCase.Execute(UserId);

            return NoContent();
        }

        [Authorize]
        [HttpGet("Events/{page}")]
        public async Task<ActionResult<PagedResult<EventVM>>> GetParticipantEvents(int page)
        {
            var events = await _getParticipantEventsUseCase.Execute(UserId, page, 10);

            return Ok(events);
        }

        [Authorize]
        [HttpGet("Events/Subscribe/{eventId}")]
        public async Task<IActionResult> SubscribeParticipantToEvent(Guid eventId)
        {
            await _subscribeToEventUseCase.Execute(UserId, eventId);

            return Ok();
        }

        [Authorize]
        [HttpGet("Events/Unsubscribe/{eventId}")]
        public async Task<IActionResult> UnsubscribeParticipantFromEvent(Guid eventId)
        {
            await _unsubscribeFromEventUseCase.Execute(UserId, eventId);

            return Ok();
        }
    }
}
