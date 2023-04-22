namespace Agora.API.Catalog.Listings;

using static Results;

static class Listings
{
    internal static void MapListings(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/listings");
        group.MapPost("/", Listings.CreateListing).RequireAuthorization();
    }

    static async Task<IResult> CreateListing(
        CreateListing listing,
        ListingService listingService
    ) => await listingService.CreateListingAsync(listing)
        .MatchOkAsync(listingId => Ok(listingId));
}