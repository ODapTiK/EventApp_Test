using FluentValidation;

namespace EventApp
{
    public class AuthParticipantValidator : AbstractValidator<AuthParticipantDAO>
    {
        public AuthParticipantValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
