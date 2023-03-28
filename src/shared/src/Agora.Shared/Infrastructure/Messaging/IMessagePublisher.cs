namespace Agora.Shared.Infrastructure.Messaging;

public interface IMessagePublisher
{
    ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}