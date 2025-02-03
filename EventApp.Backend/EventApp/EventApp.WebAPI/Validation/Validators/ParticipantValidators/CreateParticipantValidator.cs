using FluentValidation;

namespace EventApp
{
    public class CreateParticipantValidator : AbstractValidator<CreateParticipantDTO> 
    {
        public CreateParticipantValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(x => x.BirthDate).NotEmpty();
        }
    }
}
