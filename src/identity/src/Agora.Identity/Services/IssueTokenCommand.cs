using FluentValidation;

namespace Agora.Identity.Services;

public record IssueTokenCommand(string Email, string Password);

internal sealed class IssueTokenCommandValidator : AbstractValidator<IssueTokenCommand>
{
    public IssueTokenCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        // TODO: More rules here, (ideally should match the registration?).
        RuleFor(x => x.Password).NotEmpty();
    }
}