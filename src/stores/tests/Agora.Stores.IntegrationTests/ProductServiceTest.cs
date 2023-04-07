using Agora.Stores.Services;

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
    public async Task Validation_errors() // TODO: Find a better name :D.
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
            Guid.Empty,
            Guid.Empty,
            "",
            "",
            "",
            new List<ProductOption>()
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        var codes = result.Errors.Select(err => err.Code);
        Assert.Contains(nameof(ListProductCommand.StoreId), codes);
        Assert.Contains(nameof(ListProductCommand.ProductId), codes);
        Assert.Contains(nameof(ListProductCommand.Title), codes);
        Assert.Contains(nameof(ListProductCommand.Description), codes);
    }

    [Fact]
    public async Task Adding_a_listing_to_a_non_existing_store_returns_an_error()
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
            Guid.NewGuid(),
            Guid.Empty,
            "url of my product?",
            "My product",
            "This product is awesome",
            new List<ProductOption>()
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        Assert.Contains(Errors.Stores.NotFound, result.Errors);
    }

    [Fact]
    public async Task Test_2()
    {
        // Arrange
        var productService = fixture.ProductService;
        var command = new ListProductCommand(
            Guid.NewGuid(),
            Guid.NewGuid(), // TODO: Not new guid, get real store id
            "url of my product?",
            "My product",
            "This product is awesome",
            new List<ProductOption>()
        );

        // Act
        var result = await productService.AddListingAsync(command);

        // Assert
        Assert.Contains(Errors.Stores.NotFound, result.Errors);
    }
}