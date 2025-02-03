
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventAppDbContext _dbContext;

        public EventRepository(IEventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<EventModel>> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _dbContext.Events.CountAsync();
            var events = await _dbContext.Events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EventModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDateTime = e.EventDateTime,
                    Venue = e.Venue,
                    Category = e.Category,
                    MaxParticipants = e.MaxParticipants,
                    Image = e.Image,
                    Participants = e.Participants.Select(ep => new EventParticipant
                    {
                        ParticipantId = ep.ParticipantId, 
                        RegistrationDate = ep.RegistrationDate,
                        EventId = ep.EventId, 
                        Participant = new ParticipantModel
                        {
                            Name = ep.Participant.Name,
                            Surname = ep.Participant.Surname
                        }
                    }).ToList()
                })
                .ToListAsync(cancellationToken); 

            return new PagedResult<EventModel>
            {
                Items = events,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

        public async Task<EventModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events
                .Include(e => e.Participants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if (_event == null) throw new EventNotFoundException(nameof(EventModel), id);

            return _event;
        }

        public async Task<PagedResult<EventModel>> GetByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var eventsQueryable = _dbContext.Events
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(title))
                eventsQueryable = eventsQueryable.Where(e => e.Title.Contains(title.ToLower()));
            if(!string.IsNullOrWhiteSpace(category))
                eventsQueryable = eventsQueryable.Where(e => e.Category.Contains(category.ToLower()));
            if(!string.IsNullOrWhiteSpace(venue))
                eventsQueryable = eventsQueryable.Where(e => e.Venue.Contains(venue.ToLower()));
            if(!(date == DateTime.UnixEpoch))
                eventsQueryable = eventsQueryable.Where(e => e.EventDateTime.Date == date.Date);

            var totalCount = await eventsQueryable.CountAsync(cancellationToken);

            var events = await eventsQueryable
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(e => new EventModel
               {
                   Id = e.Id,
                   Title = e.Title,
                   Description = e.Description,
                   EventDateTime = e.EventDateTime,
                   Venue = e.Venue,
                   Category = e.Category,
                   MaxParticipants = e.MaxParticipants,
                   Image = e.Image,
                   Participants = e.Participants.Select(ep => new EventParticipant
                   {
                       ParticipantId = ep.ParticipantId,
                       RegistrationDate = ep.RegistrationDate,
                       EventId = ep.EventId,
                       Participant = new ParticipantModel
                       {
                           Name = ep.Participant.Name,
                           Surname = ep.Participant.Surname
                       }
                   }).ToList()
               })
               .ToListAsync(cancellationToken);

            return new PagedResult<EventModel>()
            {
                Items = events,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

       /* public async Task<EventModel> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events.FirstOrDefaultAsync(x => x.Title.Equals(title), cancellationToken);    
            if (_event == null) throw new EventNotFoundException(nameof(EventModel), title);

            return _event;
        }

        public async Task<List<EventModel>> GetEventsByCategoryAsync(string category, CancellationToken cancellationToken) => 
            await _dbContext.Events.Where(x => x.Category.Equals(category)).ToListAsync(cancellationToken);
        

        public async Task<List<EventModel>> GetEventsByDateAsync(DateTime date, CancellationToken cancellationToken) =>
            await _dbContext.Events.Where(x => x.EventDateTime.Date.Equals(date.Date)).ToListAsync(cancellationToken);

        public async Task<List<EventModel>> GetEventsByVenueAsync(string venue, CancellationToken cancellationToken) =>
            await _dbContext.Events.Where(x => x.Venue.Equals(venue)).ToListAsync(cancellationToken);*/
    }
}
