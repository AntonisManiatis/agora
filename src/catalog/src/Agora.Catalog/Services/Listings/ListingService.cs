using Agora.Catalog.Infrastructure.Data;
using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared;

using ErrorOr;

namespace Agora.Catalog.Services.Listings;

public record ListProductCommand(
    Guid Id,
    IList<byte[]> Photos, // TODO: Video too :D
    string Title,
    string Description
);

public interface IListingService
{
    Task<ErrorOr<Unit>> ListProductAsync(ListProductCommand listing);
}

sealed class ListingService : IListingService
{
    private readonly IProductRepository productRepository;

    public ListingService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ErrorOr<Unit>> ListProductAsync(ListProductCommand listing)
    {
        // TODO: Validate.

        // ? Product is not published/active yet?
        var product = new Product(
            listing.Id,
            listing.Title,
            listing.Description
        );

        await productRepository.SaveAsync(product);

        return new Unit();
    }
}