namespace Agora.Stores.Core.Products; // ! I'm not sure I agree with interfaces being in the domain area.

interface IProductRepository
{
    Task<Product?> GetAsync(ProductId productId);

    // ! Maybe should be SaveAsync
    // ! See Collection vs Persistence based repositories in the red book
    Task AddAsync(Product product);
}