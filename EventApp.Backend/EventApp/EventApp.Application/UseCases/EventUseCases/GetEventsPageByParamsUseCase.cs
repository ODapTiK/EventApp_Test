using FluentValidation;

namespace EventApp
{
    public class GetEventsPageByParamsUseCase : IGetEventsPageByParamsUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<EventFilterParams> _validator;

        public GetEventsPageByParamsUseCase(IEventRepository eventRepository, IValidator<EventFilterParams> validator)
        {
            _eventRepository = eventRepository;
            _validator = validator;
        }

        public async Task<PagedResult<EventVM>> Execute(EventFilterParams eventFilterParams, int pageSize)
        {
            var validationResult = await _validator.ValidateAsync(eventFilterParams);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            else
            {
                return await _eventRepository.GetPageByParamsAsync(
                    eventFilterParams.Title,
                    eventFilterParams.Category,
                    eventFilterParams.Venue,
                    eventFilterParams.Date,
                    eventFilterParams.Page,
                    pageSize,
                    CancellationToken.None);
            }
        }
    }
}
