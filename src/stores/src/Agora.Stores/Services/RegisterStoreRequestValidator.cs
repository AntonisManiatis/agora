using FluentValidation;

namespace Agora.Stores.Services;

internal sealed class RegisterStoreCommandValidator : AbstractValidator<RegisterStoreCommand>
{
    public RegisterStoreCommandValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}