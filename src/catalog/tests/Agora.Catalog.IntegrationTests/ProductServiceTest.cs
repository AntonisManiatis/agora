using Agora.Catalog.Services;
using Agora.Shared;

namespace Agora.Catalog.IntegrationTests;

[Collection("Catalog")]
public class ProductServiceTest : IClassFixture<ServiceFixture>
{
    private readonly ServiceFixture fixture;

    public ProductServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task Test1Async() // TODO: rename
    {
        // Arrange
        using var scope = fixture.ProductService;
        var productService = scope.Service;
        var command = new ListProductCommand(
            Guid.NewGuid(),
            new List<byte[]>(),
            "Guitar",
            "A nice guitar"
        );

        // Act
        var result = await productService.ListProductAsync(command);

        // Assert
        Assert.Equal(new Unit(), result.Value);
    }
}