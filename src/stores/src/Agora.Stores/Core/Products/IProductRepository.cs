namespace Agora.Stores.Core.Products;

interface IProductRepository
{
    Task AddAsync(Product product);
}