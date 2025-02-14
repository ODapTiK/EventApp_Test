using FluentValidation;

namespace EventApp
{
    public class CreateEventValidator : AbstractValidator<CreateEventDTO>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.MaxParticipants).NotEmpty().GreaterThan(0);
            RuleFor(x => x.EventDateTime).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        }
    }
}
