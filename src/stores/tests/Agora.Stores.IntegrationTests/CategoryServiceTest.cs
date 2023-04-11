/* TODO: Move to catalog
using Agora.Stores.Services;

namespace Agora.Stores.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class CategoryServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public CategoryServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Making_a_category_on_a_store_that_does_not_exist_returns_an_error()
    {
        // Arrange
        var categoryService = fixture.CategoryService;
        var command = new MakeCategoryCommand(
            Guid.Empty,
            "Iced coffee"
        );

        // Act
        var result = await categoryService.MakeCategoryAsync(command);

        // Assert.
        Assert.Contains(Errors.Stores.NotFound, result.Errors);
    }

    [Fact]
    public async Task Making_a_category_on_a_store_returns_an_id()
    {
        // Arrange
        var openStoreResult = await fixture.StoreService.RegisterStoreAsync(new RegisterStoreCommand
        {
            OwnerId = Guid.NewGuid(),
        });

        var storeId = openStoreResult.Value;
        var categoryService = fixture.CategoryService;
        var command = new MakeCategoryCommand(
            storeId,
            "Non iced coffee"
        );

        // Act
        var result = await categoryService.MakeCategoryAsync(command);

        // Assert
        Assert.NotEqual(0, result.Value);
    }

    [Fact]
    public async Task Making_a_duplicate_category_on_a_store_returns_an_error()
    {
        // Arrange
        var openStoreResult = await fixture.StoreService.RegisterStoreAsync(new RegisterStoreCommand
        {
            OwnerId = Guid.NewGuid(),
        });

        var storeId = openStoreResult.Value;
        var categoryService = fixture.CategoryService;
        var command = new MakeCategoryCommand(
            storeId,
            "Iced coffee"
        );

        await categoryService.MakeCategoryAsync(command);

        // Act
        var result = await categoryService.MakeCategoryAsync(command);

        // Assert.
        Assert.Contains(Errors.Categories.AlreadyExists, result.Errors);
    }
}
*/