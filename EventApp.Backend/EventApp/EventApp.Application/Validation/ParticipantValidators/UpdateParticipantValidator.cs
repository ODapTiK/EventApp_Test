using FluentValidation;

namespace EventApp
{
    public class UpdateParticipantValidator : AbstractValidator<UpdateParticipantDTO>
    {
        public UpdateParticipantValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BirthDate).NotEmpty();
        }
    }
}
