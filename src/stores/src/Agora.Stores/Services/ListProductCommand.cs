using FluentValidation;

namespace Agora.Stores.Services;

// Domain knowledge about products
// https://litextension.com/blog/difference-between-variable-products-variations-and-attributes/
// https://www.webgility.com/blog/product-attributes-options-and-variants-the-challenges

public record ProductAttribute(
    int Id,
    string Title,
    IList<ProductOption> Options
);

public record ProductOption(
    int Id,
    string Title
);

public record ListProductCommand(
    Guid ProductId,
    Guid StoreId,
    string? Photo, // Url or actual blob?
    string Title,
    string Description,
    IList<ProductOption> Options
);

sealed class ListProductCommandValidator : AbstractValidator<ListProductCommand>
{
    public ListProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.StoreId).NotEmpty();
        // TODO: Min/Max for both
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();

        // TODO: Option id not null.
        RuleForEach(p => p.Options).NotEmpty();
    }
}