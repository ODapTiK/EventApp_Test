using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EventApp
{
    [Route("api/[controller]")]
    public class EventsController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventsController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet("Id/{eventId}")]
        public async Task<ActionResult<EventVM>> GetEventById(Guid eventId)
        {
            if (eventId.Equals(Guid.Empty)) throw new FluentValidation.ValidationException("Event id must not be empty!");

            var _event = await  _eventService.GetEventByIdAsync(eventId, CancellationToken.None);   

            return Ok(_mapper.Map<EventVM>(_event));
        }

        [HttpPut("Filter")]
        public async Task<ActionResult<EventVM>> GetEventsListByParams([FromBody] EventFilterParams eventFilterParams)
        {
            var pagedEvents = await _eventService.GetEventsListByParamsAsync(
                eventFilterParams.Title,
                eventFilterParams.Category,
                eventFilterParams.Venue,
                eventFilterParams.Date,
                eventFilterParams.Page,
                10,
                CancellationToken.None);

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

        [HttpGet("{page}")]
        public async Task<ActionResult<PagedResult<EventVM>>> GetAllEvents(int page)
        {
            var pagedEvents = await _eventService.GetAllEventsListAsync(page, 10, CancellationToken.None);
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
    }
}
