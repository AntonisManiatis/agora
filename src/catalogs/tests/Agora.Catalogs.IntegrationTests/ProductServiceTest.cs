using Agora.Catalogs.Services;
using Agora.Shared;

namespace Agora.Catalogs.IntegrationTests;

[Collection("Catalog")]
public class ProductServiceTest : IClassFixture<ProductServiceFixture>
{
    private readonly ProductServiceFixture fixture;

    public ProductServiceTest(ProductServiceFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Test1Async() // TODO: rename
    {
        // Arrange
        var productService = fixture.Service;
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