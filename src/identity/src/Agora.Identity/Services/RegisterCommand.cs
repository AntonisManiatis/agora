using FluentValidation;

namespace Agora.Identity.Services;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
);

internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        // ? Max, min lengths?
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        // TODO: Obviously add other rules here.
        RuleFor(x => x.Password).NotEmpty();
    }
}