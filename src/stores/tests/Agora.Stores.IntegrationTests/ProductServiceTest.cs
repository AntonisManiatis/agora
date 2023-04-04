using Agora.Stores.Services;

using ErrorOr;

namespace Agora.Stores.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class ProductServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public ProductServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Adding_a_listing_to_a_non_existing_store_returns_an_error()
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
           Guid.Empty,
           new List<ProductListing>()
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        Assert.Contains(Error.NotFound("Store.NotFound").Code, result.Errors.Select(err => err.Code));
    }

    [Fact]
    public async Task Test_2()
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
           Guid.Empty,
           new List<ProductListing>()
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        Assert.Contains(Error.NotFound("Store.NotFound").Code, result.Errors.Select(err => err.Code));
    }
}