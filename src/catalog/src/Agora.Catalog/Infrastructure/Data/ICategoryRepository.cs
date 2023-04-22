using Agora.Catalog.Infrastructure.Data.Entities;
using Agora.Shared;

using ErrorOr;

namespace Agora.Catalog.Infrastructure.Data;

interface ICategoryRepository
{
    Task<bool> ExistsAsync(int id);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetAsync(int id, bool plusAttributes = false);

    Task<int> NextIdentity();

    Task<ErrorOr<Unit>> SaveAsync(Category category);

    Task DeleteAsync(Category category);
}