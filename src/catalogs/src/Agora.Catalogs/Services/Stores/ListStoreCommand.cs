using FluentValidation;

namespace Agora.Catalogs.Services.Stores;

public record ListStoreCommand(
    Guid Id,
    string Name,
    string? Lang
);

sealed class ListStoreCommandValidator : AbstractValidator<ListStoreCommand>
{
    public ListStoreCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        // TODO: Use a regex, basically: No special characters, spaces, or accented letters
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(20);
    }
}