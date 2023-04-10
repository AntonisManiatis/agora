using FluentValidation;

namespace Agora.Stores.Services;

public record ListProductCommand(
    Guid ProductId,
    Guid StoreId,
    string? Sku,
    int Quantity
);

sealed class ListProductCommandValidator : AbstractValidator<ListProductCommand>
{
    public ListProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.StoreId).NotEmpty();

        RuleFor(x => x.Quantity).ExclusiveBetween(0, 999);
    }
}