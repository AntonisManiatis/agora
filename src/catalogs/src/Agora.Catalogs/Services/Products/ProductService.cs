using Agora.Catalogs.Infrastructure.Data;
using Agora.Catalogs.Services.Products;
using Agora.Shared;

using ErrorOr;

namespace Agora.Catalogs.Services;

public record ListProductCommand(
    Guid Id,
    IList<byte[]> Photos, // TODO: Video too :D
    string Title,
    string Description
);

public interface IProductService
{
    Task<ErrorOr<Unit>> ListProductAsync(ListProductCommand listing);
}

sealed class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ErrorOr<Unit>> ListProductAsync(ListProductCommand listing)
    {
        // TODO: Validate.

        var product = new Product(
            listing.Id,
            listing.Title,
            listing.Description
        );

        await productRepository.SaveAsync(product);

        return new Unit();
    }
}