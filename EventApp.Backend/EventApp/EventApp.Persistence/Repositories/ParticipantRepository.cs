using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IEventAppDbContext _dbContext;

        public ParticipantRepository(IEventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Guid id, string name, string surname, string email, string password, DateTime BirthDate, CancellationToken cancellationToken)
        {
            var isParticipantExist = (await _dbContext.Participants.FirstOrDefaultAsync(x => x.Email == email)) != null;
            if (!isParticipantExist)
            {
                var participant = new ParticipantModel
                {
                    Id = id,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Password = password,
                    BirthDate = BirthDate.ToUniversalTime()
                };

                await _dbContext.Participants.AddAsync(participant);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return participant.Id;
            }
            else throw new EntityAlreadyExistsException(nameof(ParticipantModel), email);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FindAsync([id], cancellationToken);
            if (participant == null)
                throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            var eventParticipants = await _dbContext.EventParticipants
                .Where(ep => ep.ParticipantId == id)
                .ToListAsync(cancellationToken);

            /*foreach (var eventParticipant in eventParticipants)
            {
                eventParticipant.Event.Participants.Remove(eventParticipant);
            }

            _dbContext.EventParticipants.RemoveRange(eventParticipants);*/

            _dbContext.Participants.Remove(participant);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ParticipantModel> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants
                .Include(p => p.Events)
                .ThenInclude(ep => ep.Event)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            return participant;
        }

        public async Task<ParticipantModel> GetAuthentificationInfoAsync(string email, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
            if(participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), email);

            return participant;
        }

        public async Task<PagedResult<EventModel>> GetEventsAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants
                .Include(p => p.Events)
                .ThenInclude(ep => ep.Event)
                .ThenInclude(e => e.Participants) 
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            if (participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            var events = participant.Events.Select(ep => ep.Event);

            var totalCount = events.Count();

            var pagedEvents = events
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
                }).ToList(); 

            return new PagedResult<EventModel>
            {
                Items = pagedEvents,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            };
        }

        public async Task SubscribeToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if(participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            var _event = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id.Equals(eventId), cancellationToken);
            if (_event == null) throw new EventNotFoundException(nameof(EventModel), eventId);

            var subscription = new EventParticipant
            {
                EventId = eventId,
                ParticipantId = id,
                RegistrationDate = DateTime.Now.ToUniversalTime()
            };

            if (_event.Participants.Count() < _event.MaxParticipants)
            {
                /*participant.Events.Add(subscription);
                _event.Participants.Add(subscription);*/

                await _dbContext.EventParticipants.AddAsync(subscription, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else throw new ExcessNumberOfParticipantsException(_event);
        }

        public async Task UnsubscribeToEventAsync(Guid id, Guid eventId, CancellationToken cancellationToken)
        {
            var subscription = await _dbContext.EventParticipants.
                FirstOrDefaultAsync(ep => ep.ParticipantId == id && ep.EventId == eventId, cancellationToken);

            if(subscription == null) throw new LackOfEventSubscriptionException(id, eventId);

          
            _dbContext.EventParticipants.Remove(subscription);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid id, string newName, string newSurname, DateTime newBirthDate, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if(participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            participant.Name = newName;
            participant.BirthDate = newBirthDate.ToUniversalTime();
            participant.Surname = newSurname;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateRefreshTokenAsync(Guid id, string refreshToken, DateTime refreshTokenExpiryTime, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FindAsync([id], cancellationToken);
            if (participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            participant.RefreshToken = refreshToken;
            participant.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid id, string refreshToken, CancellationToken cancellationToken)
        {
            var participant = await _dbContext.Participants.FindAsync([id], cancellationToken);
            if (participant == null) throw new ParticipantNotFoundException(nameof(ParticipantModel), id);

            participant.RefreshToken = refreshToken;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
