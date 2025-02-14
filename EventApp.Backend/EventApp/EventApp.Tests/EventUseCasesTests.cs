using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public class EventUseCasesTests
    {
        private readonly EventAppDbContext _dbContext;
        private readonly EventRepository _eventRepository;
        private readonly CreateEventUseCase _createEventUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;
        private readonly GetEventByIdUseCase _getEventByIdUseCase;
        private readonly GetEventsPageUseCase _getEventsPageUseCase;
        private readonly GetEventsPageByParamsUseCase _getEventsPageByParamsUseCase;
        private readonly IMapper _mapper;

        public EventUseCasesTests()
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
            _createEventUseCase = new CreateEventUseCase(_eventRepository, new CreateEventValidator());
            _updateEventUseCase = new UpdateEventUseCase(_eventRepository, new UpdateEventValidator());
            _deleteEventUseCase = new DeleteEventUseCase(_eventRepository);
            _getEventByIdUseCase = new GetEventByIdUseCase(_eventRepository);
            _getEventsPageUseCase = new GetEventsPageUseCase(_eventRepository);
            _getEventsPageByParamsUseCase = new GetEventsPageByParamsUseCase(_eventRepository, new EventFilterParamsValidator());
        }

        [Fact]
        public async Task CreateEventAsync_ShouldAddEvent()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
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
            var addedEventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var createdEvent = await _dbContext.Events.FindAsync(addedEventId);
            Assert.NotNull(createdEvent);
            Assert.Equal(createEventDto.Title, createdEvent.Title);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldThrowValidationException_WhenCreateEventDto_NotValid()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = null,
                Category = "",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _createEventUseCase.Execute(createEventDto));
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEvent()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var eventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var updateEventDto = new UpdateEventDTO
            {
                Id = eventId,
                Title = "New Updated Event",
                Description = "Event Updated Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Updated Venue",
                Category = "Updated Category",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            await _updateEventUseCase.Execute(updateEventDto);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var updatedEvent = await _dbContext.Events.FindAsync(eventId);
            Assert.NotNull(updatedEvent);
            Assert.Equal(updateEventDto.Title, updatedEvent.Title);
            Assert.Equal(updateEventDto.Description, updatedEvent.Description);
            Assert.Equal(updateEventDto.Venue, updatedEvent.Venue);
            Assert.Equal(updateEventDto.Category, updatedEvent.Category);
            Assert.Equal(updateEventDto.EventDateTime, updatedEvent.EventDateTime);
            Assert.Equal(updateEventDto.MaxParticipants, updatedEvent.MaxParticipants);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var eventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var nonExistentEventId = Guid.NewGuid();

            var updateEventDto = new UpdateEventDTO
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
                await _updateEventUseCase.Execute(updateEventDto));
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowValidationException_WhenUpdateEventDto_NotValid()
        {
            //Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var eventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var updateEventDto = new UpdateEventDTO
            {
                Id = eventId,
                Title = "",
                Description = "New Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = null,
                Category = "",
                MaxParticipants = 50,
                Image = "imageBase64String"
            };

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _updateEventUseCase.Execute(updateEventDto));
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldDeleteEvent()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var existentEventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            await _deleteEventUseCase.Execute(existentEventId);

            // Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () => await _getEventByIdUseCase.Execute(existentEventId));
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var existentEventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act

            var nonExistentEventId = Guid.NewGuid();

            // Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () => await _deleteEventUseCase.Execute(nonExistentEventId));
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldThrowValidationException_WhenEventIdIsEmpty()
        {
            //Arrange
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var eventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var emptyId = Guid.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _deleteEventUseCase.Execute(emptyId));
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
            var result = await _getEventsPageUseCase.Execute(1, 10);

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
            var createEventDto = new CreateEventDTO
            {
                Title = "New Event",
                Description = "Event Description",
                EventDateTime = DateTime.UtcNow,
                Venue = "Venue",
                Category = "Category",
                MaxParticipants = 100,
                Image = "imageBase64String"
            };

            var eventId = await _createEventUseCase.Execute(createEventDto);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _getEventByIdUseCase.Execute(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal(result.Title, createEventDto.Title);
            Assert.Equal(result.Description, createEventDto.Description);
            Assert.Equal(result.EventDateTime, createEventDto.EventDateTime);
            Assert.Equal(result.Venue, createEventDto.Venue);
            Assert.Equal(result.Category, createEventDto.Category);
            Assert.Equal(result.MaxParticipants, createEventDto.MaxParticipants);
            Assert.Equal(result.Image, createEventDto.Image);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowEventNotFoundException_WhenNotExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<EventNotFoundException>(async () => await _getEventByIdUseCase.Execute(eventId));
        }

        [Fact]
        public async Task GetFilteredEventsAsync_ShouldReturnPagedResult()
        {
            // Arrange
            var events = new List<EventModel>
            {
                new EventModel { Id = Guid.NewGuid(), Title = "Event 1" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 2" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 3" },
                new EventModel { Id = Guid.NewGuid(), Title = "Event 4" },
                new EventModel { Id = Guid.NewGuid(), Title = "4423" },
                new EventModel { Id = Guid.NewGuid(), Title = "Ev 4" },
                new EventModel { Id = Guid.NewGuid(), Title = "Something" }
            };
            await _dbContext.Events.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var eventFilterDto = new EventFilterParams()
            {
                Title = "4",
                Date = DateTime.UnixEpoch,
                Venue = "",
                Category = "",
                Page = 1,
            };

            // Act
            var result = await _getEventsPageByParamsUseCase.Execute(eventFilterDto, 10);

            // Assert
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(3, result.Items.Count);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); 
            _dbContext.Dispose();
        }
    }
}