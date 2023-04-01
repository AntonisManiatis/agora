using FluentValidation;

namespace Agora.Stores.Services;

internal sealed class OpenStoreRequestValidator : AbstractValidator<OpenStoreCommand>
{
    public OpenStoreRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).MinimumLength(3).MaximumLength(25);
        RuleFor(x => x.TaxAddr).SetValidator(new TaxAddrValidator());
        // TODO: There has to be more.
        RuleFor(x => x.Tin).MinimumLength(9);
    }
}

internal sealed class TaxAddrValidator : AbstractValidator<TaxAddr>
{
    public TaxAddrValidator()
    {
        // TODO: Is this enough?
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
    }
}