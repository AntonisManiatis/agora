using Agora.Identity.Core;
using Agora.Identity.Services;

namespace Agora.Identity.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class AuthenticationServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public AuthenticationServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Providing_invalid_properties_returns_a_validation_error()
    {
        // Arrange
        var authenticationService = fixture.AuthenticationService;
        var invalidRequest = new AuthenticationCommand("test", "");

        // Act
        var response = await authenticationService.AuthenticateAsync(invalidRequest);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(nameof(AuthenticationCommand.Email), errorCodes);
        Assert.Contains(nameof(AuthenticationCommand.Password), errorCodes);
    }

    [Fact]
    public async Task Authentication_returns_an_error_if_user_with_given_email_does_not_exist()
    {
        // Arrange
        var authenticationService = fixture.AuthenticationService;
        var request = new AuthenticationCommand("test12321@test.net", "Apassword!");

        // Act
        var response = await authenticationService.AuthenticateAsync(request);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(Errors.InvalidCredentials.Code, errorCodes);
    }

    [Fact]
    public async Task Authentication_returns_an_error_if_user_exists_but_password_does_not_match()
    {
        // Arrange
        var authenticationService = fixture.AuthenticationService;
        var email = "test12321@test.net";
        await fixture.UserService.RegisterUserAsync(new RegisterCommand("Test", "Test", email, "Apassword"));
        var request = new AuthenticationCommand(email, "doesnotmatch");

        // Act
        var response = await authenticationService.AuthenticateAsync(request);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(Errors.InvalidCredentials.Code, errorCodes);
    }

    [Fact]
    public async Task Returns_a_token_if_credentials_are_ok()
    {
        // Arrange
        var authenticationService = fixture.AuthenticationService;
        var email = "test12321@test.net";
        var password = "Apassword";

        await fixture.UserService.RegisterUserAsync(new RegisterCommand("Test", "Test", email, password));

        var request = new AuthenticationCommand(email, password);

        // Act
        var response = await authenticationService.AuthenticateAsync(request);

        // Assert
        Assert.NotEmpty(response.Value);
    }
}