using Agora.Stores.Core.Products;
using Agora.Stores.Core.Stores;

using ErrorOr;

using FluentValidation;

namespace Agora.Stores.Services;

public interface IProductService
{
    // ? guid? or product dto?
    // Not in a rest layer but this could be relavant.
    // https://stackoverflow.com/questions/19199872/best-practice-for-restful-post-response
    Task<ErrorOr<Guid>> AddListingAsync(ListProductCommand listing);
}

sealed class ProductService : IProductService
{
    private readonly IValidator<ListProductCommand> validator;
    private readonly IStoreRepository storeRepository;
    private readonly IProductRepository productRepository;

    public ProductService(
        IValidator<ListProductCommand> validator,
        IStoreRepository storeRepository,
        IProductRepository productRepository)
    {
        this.validator = validator;
        this.storeRepository = storeRepository;
        this.productRepository = productRepository;
    }

    public async Task<ErrorOr<Guid>> AddListingAsync(ListProductCommand listing)
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

        var product = Product.List(listing.ProductId, listing.StoreId);
        product.Title = listing.Title;
        product.Description = listing.Description;
        // TODO: add options.
        // product.AddOption();

        await productRepository.AddAsync(product);

        // TODO: Handle domain events

        // ? Okay now this doesn't make sense.
        return listing.ProductId;
    }
}