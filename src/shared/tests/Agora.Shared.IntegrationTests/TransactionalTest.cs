namespace Agora.Shared.IntegrationTests;

public class TransactionalTest : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public Task Test()
    {
        return Task.CompletedTask;
    }
}