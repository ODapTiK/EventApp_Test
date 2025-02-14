using FluentValidation;

namespace EventApp
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<CreateEventDTO> _eventValidator;

        public CreateEventUseCase(IEventRepository eventRepository, IValidator<CreateEventDTO> validator)
        {
            _eventRepository = eventRepository;
            _eventValidator = validator;
        }

        public async Task<Guid> Execute(CreateEventDTO eventDto)
        {
            var validationResult = await _eventValidator.ValidateAsync(eventDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _eventRepository.CreateAsync(
                Guid.NewGuid(),
                eventDto.Title,
                eventDto.Description,
                eventDto.EventDateTime.ToUniversalTime(),
                eventDto.Venue,
                eventDto.Category,
                eventDto.MaxParticipants,
                eventDto.Image,
                CancellationToken.None);
        }
    }
}
