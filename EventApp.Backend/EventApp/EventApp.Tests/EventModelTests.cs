using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventApp
{
    public class EventModelTests
    {
        private readonly EventAppDbContext _dbContext;
        private readonly EventRepository _eventRepository;
        private readonly AdminRepository _adminRepository;

        public EventModelTests()
        {
            var options = new DbContextOptionsBuilder<EventAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new EventAppDbContext(options);
            _eventRepository = new EventRepository(_dbContext);
            _adminRepository = new AdminRepository(_dbContext);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldAddEvent()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            // Act
            await _adminRepository.CreateEventAsync(
                eventModel.Id,
                eventModel.Title, 
                eventModel.Description, 
                eventModel.EventDateTime, 
                eventModel.Venue, 
                eventModel.Category,
                eventModel.MaxParticipants,
                eventModel.Image,
                CancellationToken.None);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var createdEvent = await _dbContext.Events.FindAsync(eventModel.Id);
            Assert.NotNull(createdEvent);
            Assert.Equal(eventModel.Title, createdEvent.Title);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEvent()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            await _adminRepository.CreateEventAsync(
                eventModel.Id,
                eventModel.Title,
                eventModel.Description,
                eventModel.EventDateTime,
                eventModel.Venue,
                eventModel.Category,
                eventModel.MaxParticipants,
                eventModel.Image,
                CancellationToken.None);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var newEventModel = new EventModel
            {
                Id = eventModel.Id,
                Title = "New Updated Event",
                Description = "Event Updated Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Updated Venue",
                Category = "Updated Category",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            await _adminRepository.UpdateEventAsync(
                newEventModel.Id,
                newEventModel.Title,
                newEventModel.Description,
                newEventModel.EventDateTime,
                newEventModel.Venue,
                newEventModel.Category,
                newEventModel.MaxParticipants,
                newEventModel.Image,
                CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var updatedEvent = await _dbContext.Events.FindAsync(eventModel.Id);
            Assert.NotNull(updatedEvent);
            Assert.Equal(newEventModel.Title, updatedEvent.Title);
            Assert.Equal(newEventModel.Description, updatedEvent.Description);
            Assert.Equal(newEventModel.Venue, updatedEvent.Venue);
            Assert.Equal(newEventModel.Category, updatedEvent.Category);
            Assert.Equal(newEventModel.EventDateTime, updatedEvent.EventDateTime);
            Assert.Equal(newEventModel.MaxParticipants, newEventModel.MaxParticipants);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            await _adminRepository.CreateEventAsync(
                eventModel.Id,
                eventModel.Title,
                eventModel.Description,
                eventModel.EventDateTime,
                eventModel.Venue,
                eventModel.Category,
                eventModel.MaxParticipants,
                eventModel.Image,
                CancellationToken.None);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var nonExistentEventId = Guid.NewGuid();

            var newEventModel = new EventModel
            {
                Id = nonExistentEventId,
                Title = "New Updated Event",
                Description = "Event Updated Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Updated Venue",
                Category = "Updated Category",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            // Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () =>
                await _adminRepository.UpdateEventAsync(
                    newEventModel.Id,
                    newEventModel.Title,
                    newEventModel.Description,
                    newEventModel.EventDateTime,
                    newEventModel.Venue,
                    newEventModel.Category,
                    newEventModel.MaxParticipants,
                    newEventModel.Image,
                    CancellationToken.None));
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldDeleteEvent()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            await _adminRepository.CreateEventAsync(
                eventModel.Id,
                eventModel.Title,
                eventModel.Description,
                eventModel.EventDateTime,
                eventModel.Venue,
                eventModel.Category,
                eventModel.MaxParticipants,
                eventModel.Image,
                CancellationToken.None);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var existentEventId = eventModel.Id;

            var newEventModel = new EventModel
            {
                Id = existentEventId,
                Title = "New Updated Event",
                Description = "Event Updated Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Updated Venue",
                Category = "Updated Category",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            await _adminRepository.DeleteEventAsync(existentEventId, CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () => await _eventRepository.GetByIdAsync(existentEventId, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            await _adminRepository.CreateEventAsync(
                eventModel.Id,
                eventModel.Title,
                eventModel.Description,
                eventModel.EventDateTime,
                eventModel.Venue,
                eventModel.Category,
                eventModel.MaxParticipants,
                eventModel.Image,
                CancellationToken.None);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var nonExistentEventId = Guid.NewGuid();

            var newEventModel = new EventModel
            {
                Id = nonExistentEventId,
                Title = "New Updated Event",
                Description = "Event Updated Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Updated Venue",
                Category = "Updated Category",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            // Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () => await _adminRepository.DeleteEventAsync(nonExistentEventId, CancellationToken.None));
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnPagedResult()
        {
            // Arrange
            var events = new List<EventModel>
            {
                new EventModel { Id = Guid.NewGuid(), Title = "Event 1" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 2" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 3" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 4" }
            };
            await _dbContext.Events.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _eventRepository.GetAllEventsAsync(1, 10, CancellationToken.None);

            // Assert
            Assert.Equal(4, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(events.Count, result.Items.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventModel = new EventModel { Id = eventId, Title = "Event 1" };
            await _dbContext.Events.AddAsync(eventModel);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _eventRepository.GetByIdAsync(eventId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<EventNotFoundException>(() => _eventRepository.GetByIdAsync(eventId, CancellationToken.None));
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); 
            _dbContext.Dispose();
        }
    }
}