using Agora.Identity.Core;
using Agora.Identity.Services;

namespace Agora.Identity.IntegrationTests;

[Collection("Identity")]
public class TokenServiceTest
{
    private readonly ServiceFixture fixture;

    public TokenServiceTest(ServiceFixture fixture) => this.fixture = fixture;

    [Fact]
    public async Task Providing_invalid_properties_returns_a_validation_error()
    {
        // Arrange
        using var scope = fixture.TokenService;
        var tokenService = scope.Service;
        var invalidRequest = new IssueTokenCommand("test", "");

        // Act
        var response = await tokenService.IssueAsync(invalidRequest);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(nameof(IssueTokenCommand.Email), errorCodes);
        Assert.Contains(nameof(IssueTokenCommand.Password), errorCodes);
    }

    [Fact]
    public async Task Authentication_returns_an_error_if_user_with_given_email_does_not_exist()
    {
        // Arrange
        using var scope = fixture.TokenService;
        var tokenService = scope.Service;
        var request = new IssueTokenCommand("test12321@test.net", "Apassword!");

        // Act
        var response = await tokenService.IssueAsync(request);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(Errors.InvalidCredentials.Code, errorCodes);
    }

    [Fact]
    public async Task Authentication_returns_an_error_if_user_exists_but_password_does_not_match()
    {
        // Arrange
        using var scope = fixture.TokenService;
        var tokenService = scope.Service;

        var email = "test12321@test.net";
        using (var scope2 = fixture.UserService)
        {
            await scope2.Service.RegisterUserAsync(new RegisterCommand("Test", "Test", email, "Apassword"));
        }

        var request = new IssueTokenCommand(email, "doesnotmatch");

        // Act
        var response = await tokenService.IssueAsync(request);

        // Assert
        var errorCodes = response.Errors.Select(err => err.Code);
        Assert.Contains(Errors.InvalidCredentials.Code, errorCodes);
    }

    [Fact]
    public async Task Returns_a_token_if_credentials_are_ok()
    {
        // Arrange
        using var scope = fixture.TokenService;
        var tokenService = scope.Service;

        var email = "test12321@test.net";
        var password = "Apassword";

        using (var scope2 = fixture.UserService)
        {
            await scope2.Service.RegisterUserAsync(new RegisterCommand("Test", "Test", email, password));
        }

        var request = new IssueTokenCommand(email, password);

        // Act
        var response = await tokenService.IssueAsync(request);

        // Assert
        Assert.NotEmpty(response.Value);
    }
}