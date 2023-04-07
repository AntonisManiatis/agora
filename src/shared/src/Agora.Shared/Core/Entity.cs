namespace Agora.Shared.Core;

public abstract class Entity
{
    private static readonly List<object> Empty = new List<object>();

    private List<object> events = Empty;

    public IReadOnlyList<object> Events => events;

    protected void AddEvent(object @event)
    {
        if (events == Empty)
        {
            events = new();
        }

        events.Add(@event);
    }

    public void ClearEvents()
    {
        events.Clear();
    }
}