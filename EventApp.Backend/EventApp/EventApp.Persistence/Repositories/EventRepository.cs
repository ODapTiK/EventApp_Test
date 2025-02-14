
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventRepository(IEventAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(Guid id, string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string image, CancellationToken cancellationToken)
        {
            var _event = new EventModel
            {
                Id = id,
                Title = title,
                Description = description,
                EventDateTime = eventDateTime,
                Venue = venue,
                Category = category,
                MaxParticipants = maxParticipants,
                Image = image,
            };

            await _dbContext.Events.AddAsync(_event, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _event.Id;
        }

        public async Task DeleteAsync(EventModel _event, CancellationToken cancellationToken)
        {
            _dbContext.Events.Remove(_event);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(EventModel _event, Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string newImage, CancellationToken cancellationToken)
        {
            _event.Title = newTitle;
            _event.Description = newDescription;
            _event.EventDateTime = newEventDateTime;
            _event.Venue = newVenue;
            _event.Category = newCategory;
            _event.MaxParticipants = newMaxParticipants;
            _event.Image = newImage;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<EventVM>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _dbContext.Events.CountAsync();
            var events = await _dbContext.Events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken); 

            return new PagedResult<EventVM>
            {
                Items = _mapper.Map<List<EventVM>>(events),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

        public async Task<EventVM> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events
                .Include(e => e.Participants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return _mapper.Map<EventVM>(_event);
        }

        public async Task<EventModel?> FindEventAsync(Guid id, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events
                .FindAsync([id], cancellationToken);

            return _event;
        }

        public async Task<PagedResult<EventVM>> GetPageByParamsAsync(string title, string category, string venue, DateTime date, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var eventsQueryable = _dbContext.Events
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(title))
                eventsQueryable = eventsQueryable.Where(e => e.Title.ToLower().Contains(title.ToLower()));
            if(!string.IsNullOrWhiteSpace(category))
                eventsQueryable = eventsQueryable.Where(e => e.Category.ToLower().Contains(category.ToLower()));
            if(!string.IsNullOrWhiteSpace(venue))
                eventsQueryable = eventsQueryable.Where(e => e.Venue.ToLower().Contains(venue.ToLower()));
            if(!(date == DateTime.UnixEpoch))
                eventsQueryable = eventsQueryable.Where(e => e.EventDateTime.Date == date.Date);

            var totalCount = await eventsQueryable.CountAsync(cancellationToken);

            var events = await eventsQueryable
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(cancellationToken);

            return new PagedResult<EventVM>()
            {
                Items = _mapper.Map<List<EventVM>>(events),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

    }
}
