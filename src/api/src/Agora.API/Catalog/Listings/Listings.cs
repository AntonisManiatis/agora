namespace Agora.API.Catalog.Listings;

using static Results;

static class Listings
{
    internal static void MapListings(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/listings");
        group.MapPost("/", Listings.CreateListing)
            .RequireAuthorization();
    }

    internal static async Task<IResult> CreateListing(
        CreateListing listing,
        ListingService listingService
    )
    {
        var result = await listingService.CreateListingAsync(listing);

        return result.Match<IResult>(
            listingId => Ok(listingId),
            errors => errors.ToProblem()
        );
    }
}