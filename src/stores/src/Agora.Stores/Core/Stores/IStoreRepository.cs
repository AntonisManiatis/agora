namespace Agora.Stores.Core.Stores;

interface IStoreRepository
{
    Task<Store?> GetStoreAsync(Guid storeId); // TODO: Use value object

    Task<IEnumerable<Store>> GetStoresAsync();

    Task AddAsync(Store store);
}