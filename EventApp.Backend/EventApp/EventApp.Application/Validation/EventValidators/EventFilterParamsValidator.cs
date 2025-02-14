using FluentValidation;

namespace EventApp
{
    public class EventFilterParamsValidator : AbstractValidator<EventFilterParams>
    {
        public EventFilterParamsValidator()
        {
            RuleFor(x => x.Page).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
