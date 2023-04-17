namespace Agora.API.Catalog.Listings;

static class ListingsMappings
{
    internal static void MapListings(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/listings");
        group.MapPost("/", Listings.CreateListing)
            .RequireAuthorization();
    }
}