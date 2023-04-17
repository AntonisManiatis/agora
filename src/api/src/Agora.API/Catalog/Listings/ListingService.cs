using Agora.Shared;

using ErrorOr;

namespace Agora.API.Catalog.Listings;

class ListingService
{
    [Transactional]
    public virtual Task<ErrorOr<Guid>> CreateListingAsync(CreateListing listing)
    {
        throw new NotImplementedException();
    }
}