using FluentValidation;

namespace EventApp
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<UpdateEventDTO> _updateEventValidator;

        public UpdateEventUseCase(IEventRepository eventRepository, IValidator<UpdateEventDTO> updateEventValidator)
        {
            _eventRepository = eventRepository;
            _updateEventValidator = updateEventValidator;
        }

        public async Task Execute(UpdateEventDTO updateEventDTO)
        {
            var validationResult = await _updateEventValidator.ValidateAsync(updateEventDTO);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var _event = await _eventRepository.FindEventAsync(updateEventDTO.Id, CancellationToken.None);
            if (_event == null) throw new EventNotFoundException(nameof(updateEventDTO), updateEventDTO.Id);
            else
            {
                _event.Title = updateEventDTO.Title;
                _event.Description = updateEventDTO.Description;
                _event.EventDateTime = updateEventDTO.EventDateTime.ToUniversalTime();
                _event.Venue = updateEventDTO.Venue;
                _event.Category = updateEventDTO.Category;
                _event.MaxParticipants = updateEventDTO.MaxParticipants;
                _event.Image = updateEventDTO.Image;
                await _eventRepository.UpdateAsync(
                    CancellationToken.None);
            }
        }
    }
}
