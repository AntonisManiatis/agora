using System.Data;

namespace Agora.Shared;

/// <summary>
/// Any method marked as transactional
/// will begin a transaction and complete if the method exits normally
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TransactionalAttribute : Attribute
{
    public IsolationLevel? IsolationLevel { get; }

    public TransactionalAttribute(IsolationLevel isolationLevel)
    {
        IsolationLevel = isolationLevel;
    }

    public TransactionalAttribute() { }
}