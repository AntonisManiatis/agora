using Agora.Catalog.Services.Categories;

namespace Agora.Catalog.IntegrationTests;

[Collection("Catalog")]
public class CategoryServiceTest
{
    private readonly ServiceFixture fixture;

    public CategoryServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task A_category_requires_a_unique_name()
    {
        // Arrange
        using var scope = fixture.CategoryService;
        var categoryService = scope.Service;

        var req = new CreateCategoryCommand("Clothes");
        _ = await categoryService.CreateAsync(req);

        // Act
        var result = await categoryService.CreateAsync(req);

        // Assert
        Assert.Contains(Errors.AlreadyExists, result.Errors);
    }

    [Fact]
    public async Task Creating_a_category_returns_a_positive_id()
    {
        // Arrange
        using var scope = fixture.CategoryService;
        var categoryService = scope.Service;

        // Act
        var result = await categoryService.CreateAsync(
            new CreateCategoryCommand(
                "Phones"
            )
        );

        // Assert
        Assert.True(result.Value > 0);
    }

    [Fact]
    public async Task Creating_a_nested_category_returns_an_error_if_the_parent_does_not_exist()
    {
        // Arrange
        using var scope = fixture.CategoryService;
        var categoryService = scope.Service;

        // Act
        var result = await categoryService.CreateAsync(
            new CreateCategoryCommand(
                "Laptops",
                "A lot of laptops.",
                ParentId: 999 // Non existant parent category.
            )
        );

        // Assert
        Assert.Contains(Errors.ParentNotFound, result.Errors);
    }

    [Fact]
    public async Task Getting_a_category_that_does_not_exist_returns_an_error()
    {
        // Arrange
        using var scope = fixture.CategoryService;
        var categoryService = scope.Service;

        // Act
        var result = await categoryService.GetAsync(9999);

        // Assert
        Assert.Contains(Errors.CategoryNotFound, result.Errors);
    }

    [Fact]
    public async Task Getting_a_category_() // TODO: Rename, basically this test says okay fields match
    {
        // Arrange
        using var scope = fixture.CategoryService;
        var categoryService = scope.Service;

        var r = await categoryService.CreateAsync(
            new CreateCategoryCommand(
                "Computers",
                "A lot of computers."
            )
        );

        var existingCategoryId = r.Value;

        using var scope2 = fixture.CategoryService;
        categoryService = scope2.Service;

        // Act
        var result = await categoryService.GetAsync(existingCategoryId);

        // Assert
        Assert.Equal(new Category(existingCategoryId, "Computers", "A lot of computers."), result.Value);
    }
}