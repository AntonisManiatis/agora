using FluentValidation;

namespace Agora.Identity.Services;

public record AuthenticationCommand(string Email, string Password);

internal sealed class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        // TODO: More rules here, (ideally should match the registration?).
        RuleFor(x => x.Password).NotEmpty();
    }
}