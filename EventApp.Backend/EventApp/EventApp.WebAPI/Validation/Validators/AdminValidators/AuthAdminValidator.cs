using FluentValidation;

namespace EventApp
{
    public class AuthAdminValidator : AbstractValidator<AuthAdminDAO>
    {
        public AuthAdminValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
