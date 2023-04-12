using Agora.Catalogs.Infrastructure.Data.Entities;

namespace Agora.Catalogs.Infrastructure.Data;

interface IProductRepository
{
    Task SaveAsync(Product product);
}