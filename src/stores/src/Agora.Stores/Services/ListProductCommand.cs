using FluentValidation;

namespace Agora.Stores.Services;

public record ProductListing(
    string? Photo, // Url or actual blob?
    string Title,
    string Description // TODO: Add options, attributes or variations?
);

public record ListProductCommand(
    Guid StoreId,
    IList<ProductListing> Listings
);

sealed class ListProductCommandValidator : AbstractValidator<ListProductCommand>
{
    public ListProductCommandValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        // TODO: Cannot find a way to validate collections.
        // RuleFor(x => x.Listings).SetValidator(new ProductListingValidator());
    }
}

sealed class ProductListingValidator : AbstractValidator<ProductListing>
{
    public ProductListingValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}