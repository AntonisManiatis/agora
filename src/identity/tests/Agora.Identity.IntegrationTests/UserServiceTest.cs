using Agora.Identity.Services;

using ErrorOr;

namespace Agora.Identity.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class UserServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public UserServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Registering_a_user_returns_a_unique_id()
    {
        // Arrange
        var userService = fixture.UserService;
        var request = new RegisterCommand("Anthony", "Maniatis", "test@test.net", "Apassword!");

        // Act
        var result = await userService.RegisterUserAsync(request);

        // Assert
        Assert.NotEqual(default, result.Value);
    }

    [Fact]
    public async Task Providing_one_or_more_invalid_properties_returns_an_error()
    {
        // Arrange
        var userService = fixture.UserService;
        var invalidRequest = new RegisterCommand("", "", "test.com", "");

        // Act
        var result = await userService.RegisterUserAsync(invalidRequest);

        // Assert
        var codes = result.Errors.Select(err => err.Code);
        Assert.Contains(nameof(RegisterCommand.FirstName), codes);
        Assert.Contains(nameof(RegisterCommand.LastName), codes);
        Assert.Contains(nameof(RegisterCommand.Email), codes);
        Assert.Contains(nameof(RegisterCommand.Password), codes);
    }

    [Fact]
    public async Task Registering_a_user_with_an_existing_email_returns_error()
    {
        // Arrange
        var userService = fixture.UserService;
        _ = await userService.RegisterUserAsync(new RegisterCommand("Anthony", "Maniatis", "test2@test.net", "Apassword!"));

        var request = new RegisterCommand("Anthony", "Maniatis", "test2@test.net", "Apassword!");

        // Act
        var result = await userService.RegisterUserAsync(request);

        // Assert
        Assert.Contains(Error.Conflict(), result.Errors);
    }
}