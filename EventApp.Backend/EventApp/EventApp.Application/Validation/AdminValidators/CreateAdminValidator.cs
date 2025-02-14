using FluentValidation;

namespace EventApp
{
    public class CreateAdminValidator : AbstractValidator<CreateAdminDTO>
    {
        public CreateAdminValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
