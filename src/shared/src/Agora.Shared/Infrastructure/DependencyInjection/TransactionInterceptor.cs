using Castle.DynamicProxy;

namespace Agora.Shared.Infrastructure.DependencyInjection;

sealed class TransactionInterceptor : IInterceptor
{
    private readonly AsyncTransactionInterceptor asyncTransactionInterceptor;

    public TransactionInterceptor(AsyncTransactionInterceptor asyncTransactionInterceptor) =>
        this.asyncTransactionInterceptor = asyncTransactionInterceptor;

    public void Intercept(IInvocation invocation) =>
        asyncTransactionInterceptor.ToInterceptor().Intercept(invocation);
}