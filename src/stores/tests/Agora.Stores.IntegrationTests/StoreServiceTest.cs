namespace Agora.Stores.IntegrationTests;

[Collection(nameof(PostgreSqlFixture))]
public class StoreServiceTest
{
    private readonly PostgreSqlFixture fixture;

    public StoreServiceTest(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var storeService = fixture.Service;

        var req = new OpenStoreRequest
        {
            UserId = Guid.NewGuid(),
            Name = "Coffee Lab",
            StoreAddr = new StoreAddr
            {

            }
        };

        // Act
        var result = await storeService.SubmitOpenStoreRequestAsync(req);

        // Assert
        result.Switch(
            applicationId => Assert.NotEqual(default, applicationId),
            _ => { }
        );
    }
}