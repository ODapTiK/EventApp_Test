
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IEventAppDbContext _dbContext;

        public AdminRepository(IEventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Guid id, string email, string password, CancellationToken cancellationToken)
        {
            var isAdminExists = (await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email == email)) != null;
            if (!isAdminExists)
            {
                var admin = new AdminModel
                {
                    Id = id,
                    Email = email,
                    Password = password
                };

                await _dbContext.Admins.AddAsync(admin, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return admin.Id;
            }
            else
            {
                 throw new EntityAlreadyExistsException(nameof(AdminModel), email);
            }
        }

        public async Task<Guid> CreateEventAsync(Guid id, string title, string description, DateTime eventDateTime, string venue, string category, int maxParticipants, string image, CancellationToken cancellationToken)
        {
            var _event = new EventModel
            {
                Id = id,
                Title = title,
                Description = description,
                EventDateTime = eventDateTime.ToUniversalTime(),
                Venue = venue,
                Category = category,
                MaxParticipants = maxParticipants,
                Image = image,
            };

            await _dbContext.Events.AddAsync(_event, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _event.Id;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FindAsync([id], cancellationToken); 
            if (admin == null) 
                throw new AdminNotFoundException(nameof(AdminModel), id);

            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events.FindAsync([eventId], cancellationToken);
            if(_event == null) 
                throw new EventNotFoundException(nameof(EventModel), eventId);

            var eventPartisipants = await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .ToListAsync(cancellationToken);

            /*foreach(var ep in eventPartisipants)
            {
                ep.Participant.Events.Remove(ep);
            }

            _dbContext.EventParticipants.RemoveRange(eventPartisipants);*/
            _dbContext.Events.Remove(_event);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<AdminModel> GetAdminAuthentificationDataAsync(string adminEmail, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email.Equals(adminEmail), cancellationToken);
            if(admin == null) throw new AdminNotFoundException(nameof(AdminModel), adminEmail);

            return admin;
        }

        public async Task<AdminModel> GetAdminByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if (admin == null) throw new AdminNotFoundException(nameof(AdminModel), id);

            return admin;
        }

        public async Task UpdateEventAsync(Guid eventId, string newTitle, string newDescription, DateTime newEventDateTime, string newVenue, string newCategory, int newMaxParticipants, string newImage, CancellationToken cancellationToken)
        {
            var _event = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id.Equals(eventId), cancellationToken);
            if (_event == null) throw new EventNotFoundException(nameof(EventModel), eventId);

            _event.Title = newTitle;
            _event.Description = newDescription;
            _event.EventDateTime = newEventDateTime.ToUniversalTime();
            _event.Venue = newVenue;
            _event.Category = newCategory;
            _event.MaxParticipants = newMaxParticipants;
            _event.Image = newImage;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid id, string refreshToken, DateTime refreshTokenExpiryTime, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FindAsync([id], cancellationToken);
            if (admin == null) throw new AdminNotFoundException(nameof(AdminModel), id);

            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid id, string refreshToken, CancellationToken cancellationToken)
        {
            var admin = await _dbContext.Admins.FindAsync([id], cancellationToken);
            if (admin == null) throw new AdminNotFoundException(nameof(AdminModel), id);

            admin.RefreshToken = refreshToken;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
