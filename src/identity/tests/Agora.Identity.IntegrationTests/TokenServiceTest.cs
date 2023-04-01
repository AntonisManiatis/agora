using Agora.Identity.Core;
using Agora.Identity.Services;

namespace Agora.Identity.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class TokenServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public TokenServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Providing_invalid_properties_returns_a_validation_error()
    {
        // Arrange
        var tokenService = fixture.TokenService;
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
        var tokenService = fixture.TokenService;
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
        var tokenService = fixture.TokenService;
        var email = "test12321@test.net";
        await fixture.UserService.RegisterUserAsync(new RegisterCommand("Test", "Test", email, "Apassword"));
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
        var tokenService = fixture.TokenService;
        var email = "test12321@test.net";
        var password = "Apassword";

        await fixture.UserService.RegisterUserAsync(new RegisterCommand("Test", "Test", email, password));

        var request = new IssueTokenCommand(email, password);

        // Act
        var response = await tokenService.IssueAsync(request);

        // Assert
        Assert.NotEmpty(response.Value);
    }
}