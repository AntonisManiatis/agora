using Agora.Shared;
using Agora.Stores.Core.Products;
using Agora.Stores.Core.Stores;

using ErrorOr;

using FluentValidation;

using Mapster;

namespace Agora.Stores.Services;

public record Product(
    Guid Id,
    Guid StoreId,
    string? Sku,
    int Quantity
);

public interface IInventoryService
{
    // ? Not entirely convinced with the name
    Task<ErrorOr<Unit>> AddListingAsync(ListProductCommand listing);

    Task<ErrorOr<Product>> GetProductAsync(Guid productId);
}

sealed class InventoryService : IInventoryService
{
    private readonly IValidator<ListProductCommand> validator;
    private readonly IStoreRepository storeRepository;
    private readonly IProductRepository productRepository;

    public InventoryService(
        IValidator<ListProductCommand> validator,
        IStoreRepository storeRepository,
        IProductRepository productRepository)
    {
        this.validator = validator;
        this.storeRepository = storeRepository;
        this.productRepository = productRepository;
    }

    public async Task<ErrorOr<Product>> GetProductAsync(Guid productId)
    {
        var product = await productRepository.GetAsync(productId);
        if (product is null)
        {
            return Errors.Products.NotFound; // ? include id?
        }

        return product.Adapt<Product>();
    }

    public async Task<ErrorOr<Unit>> AddListingAsync(ListProductCommand listing)
    {
        // TODO: Find a trick to specify it one for all services?
        var result = await validator.ValidateAsync(listing);
        if (!result.IsValid)
        {
            return result.Errors
                .ConvertAll(err => Error.Validation(err.PropertyName, err.ErrorMessage));
        }

        var store = await storeRepository.GetStoreAsync(listing.StoreId);
        if (store is null)
        {
            return Errors.Stores.NotFound; // ? Include id?
        }

        var product = Core.Products.Product.List(
            listing.ProductId,
            listing.StoreId,
            listing.Sku,
            listing.Quantity
        );

        // We care about errors of List
        var r = await product.MatchAsync<List<Error>>(
            async product =>
            {
                await productRepository.AddAsync(product);
                return new List<Error>(); // ! allocation, yikes
            },
            errors => Task.FromResult(errors)
        );

        // TODO: Handle domain events

        return new Unit();
    }
}