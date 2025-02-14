using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp
{
    [Route("api/[controller]")]
    public class EventsController : BaseController
    {
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;
        private readonly IGetEventByIdUseCase _getEventByIdUseCase;
        private readonly IGetEventsPageUseCase _getEventsPageUseCase;
        private readonly IGetEventsPageByParamsUseCase _getEventsPageByParamsUseCase;

        public EventsController(ICreateEventUseCase createEventUseCase,
                                IUpdateEventUseCase updateEventUseCase,
                                IDeleteEventUseCase deleteEventUseCase,
                                IGetEventByIdUseCase getEventByIdUseCase,
                                IGetEventsPageUseCase getEventsPageUseCase,
                                IGetEventsPageByParamsUseCase getEventsPageByParamsUseCase)
        {
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _getEventByIdUseCase = getEventByIdUseCase;
            _getEventsPageUseCase = getEventsPageUseCase;
            _getEventsPageByParamsUseCase = getEventsPageByParamsUseCase;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateEvent([FromBody] CreateEventDTO createEventDTO)
        {
            var eventId = await _createEventUseCase.Execute(createEventDTO);

            return Ok(eventId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO updateEventDTO)
        {
            await _updateEventUseCase.Execute(updateEventDTO);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _deleteEventUseCase.Execute(id);

            return Ok();
        }

        [HttpGet("Id/{eventId}")]
        public async Task<ActionResult<EventVM>> GetEventById(Guid eventId)
        {
            var _event = await _getEventByIdUseCase.Execute(eventId); 

            return Ok(_event);
        }

        [HttpPut("Filter")]
        public async Task<ActionResult<PagedResult<EventVM>>> GetEventsListByParams([FromBody] EventFilterParams eventFilterParams)
        {
            var events = await _getEventsPageByParamsUseCase.Execute(eventFilterParams, 10);

            return Ok(events);
        }

        [HttpGet("{page}")]
        public async Task<ActionResult<PagedResult<EventVM>>> GetAllEvents(int page)
        {
            var events = await _getEventsPageUseCase.Execute(page, 10);

            return Ok(events);
        }
    }
}
