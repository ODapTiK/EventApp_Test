using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class EventRepositoryTests
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<EventAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TestMapperProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _dbContext = new EventAppDbContext(options);
            _eventRepository = new EventRepository(_dbContext, _mapper);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldAddEvent()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            // Act
            var addedEventId = await _eventRepository.CreateAsync(
                Guid.NewGuid(),
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
            var createdEvent = await _dbContext.Events.FindAsync(addedEventId);
            Assert.NotNull(createdEvent);
            Assert.Equal(eventModel.Title, createdEvent.Title);
            Assert.Equal(eventModel.Description, createdEvent.Description);
            Assert.Equal(eventModel.EventDateTime, createdEvent.EventDateTime);
            Assert.Equal(eventModel.Venue, createdEvent.Venue);
            Assert.Equal(eventModel.Category, createdEvent.Category);
            Assert.Equal(eventModel.MaxParticipants, createdEvent.MaxParticipants); 
            Assert.Equal(eventModel.Image, createdEvent.Image);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenExists()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var addedEventId = await _eventRepository.CreateAsync(
                Guid.NewGuid(),
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
            var result = await _eventRepository.GetByIdAsync(addedEventId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addedEventId, result.Id);
            Assert.Equal(result.Title, eventModel.Title);
            Assert.Equal(result.Description, eventModel.Description);
            Assert.Equal(result.EventDateTime, eventModel.EventDateTime);
            Assert.Equal(result.Venue, eventModel.Venue);
            Assert.Equal(result.Category, eventModel.Category);
            Assert.Equal(result.MaxParticipants, eventModel.MaxParticipants);
            Assert.Equal(eventModel.Image, result.Image);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var eventModel = new EventModel
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var addedEventId = await _eventRepository.CreateAsync(
                Guid.NewGuid(),
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
            var result = await _eventRepository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
