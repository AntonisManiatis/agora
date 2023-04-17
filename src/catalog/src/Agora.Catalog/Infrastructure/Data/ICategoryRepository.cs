using Agora.Catalog.Infrastructure.Data.Entities;

namespace Agora.Catalog.Infrastructure.Data;

interface ICategoryRepository
{
    Task<bool> ExistsAsync(int id);

    Task<Category?> GetAsync(int id);

    Task<int> NextIdentity();

    Task SaveAsync(Category category);

    Task DeleteAsync(Category category);
}