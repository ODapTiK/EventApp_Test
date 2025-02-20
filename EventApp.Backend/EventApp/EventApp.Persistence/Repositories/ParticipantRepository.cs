using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IEventAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ParticipantRepository(IEventAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(Guid id, string name, string surname, string email, string password, DateTime BirthDate, CancellationToken cancellationToken)
        {
            var participant = new ParticipantModel
            {
                Id = id,
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                BirthDate = BirthDate
            };

            await _dbContext.Participants.AddAsync(participant);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return participant.Id;
        }

        public async Task DeleteAsync(ParticipantModel participant, CancellationToken cancellationToken)
        {
            _dbContext.Participants.Remove(participant);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ParticipantModel?> FindParticipantAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Participants.FindAsync([id], cancellationToken);
        }

        public async Task<EventParticipant?> FindSubscriptionAsync(Guid participantId, Guid eventId, CancellationToken cancellationToken)
        {
            var subscription = await _dbContext.EventParticipants.
                FirstOrDefaultAsync(ep => ep.ParticipantId == participantId && ep.EventId == eventId, cancellationToken);

            return subscription;    
        }

        public async Task<ParticipantModel?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants
                .Include(p => p.Events)
                .ThenInclude(ep => ep.Event)
                .ThenInclude(e => e.Participants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return participant;
        }

        public async Task<ParticipantModel?> GetAuthDataAsync(string email, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);

            return participant;
        }

        public async Task<PagedResult<EventVM>> GetEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants
                .Include(p => p.Events)
                .ThenInclude(ep => ep.Event)
                .ThenInclude(e => e.Participants) 
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);


            var events = participant.Events.Select(ep => ep.Event);

            var totalCount = events.Count();

            var pagedEvents = events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); 

            return new PagedResult<EventVM>
            {
                Items = _mapper.Map<List<EventVM>>(pagedEvents),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

        public async Task SubscribeToEventAsync(Guid id, Guid eventId, DateTime registrationDateTime, CancellationToken cancellationToken)
        {
            var subscription = new EventParticipant
            {
                EventId = eventId,
                ParticipantId = id,
                RegistrationDate = registrationDateTime
            };

            await _dbContext.EventParticipants.AddAsync(subscription, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnsubscribeToEventAsync(EventParticipant subscription, CancellationToken cancellationToken)
        {
            _dbContext.EventParticipants.Remove(subscription);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
