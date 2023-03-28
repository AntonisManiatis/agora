namespace Agora.Shared.Infrastructure.Messaging;

internal sealed class NullMessagePublisher : IMessagePublisher
{
    public ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        // No op. 
        // TODO: Maybe logging it?
        return ValueTask.CompletedTask;
    }
}