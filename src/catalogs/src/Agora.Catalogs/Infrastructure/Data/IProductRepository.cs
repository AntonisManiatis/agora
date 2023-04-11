using Agora.Catalogs.Services.Products;

namespace Agora.Catalogs.Infrastructure.Data;

interface IProductRepository
{
    Task SaveAsync(Product product);
}