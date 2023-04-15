using Agora.Catalog.Infrastructure.Data.Entities;

namespace Agora.Catalog.Infrastructure.Data;

interface IProductRepository
{
    Task SaveAsync(Product product);
}