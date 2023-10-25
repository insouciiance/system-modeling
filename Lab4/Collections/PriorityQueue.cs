using System.Diagnostics.CodeAnalysis;
using BCL = System.Collections.Generic;

namespace Lab4.Collections;

public class PriorityQueue<TItem, TPriority> : IQueue<TItem>
{
    private readonly BCL.PriorityQueue<TItem, TPriority> _queue = new();

    private readonly IPrioritySelector<TItem, TPriority> _prioritySelector;

    public PriorityQueue(IPrioritySelector<TItem, TPriority> prioritySelector)
    {
        _prioritySelector = prioritySelector;
    }

    public int Count => _queue.Count;

    public virtual bool TryEnqueue(TItem item)
    {
        var priority = _prioritySelector.GetPriority(item);
        _queue.Enqueue(item, priority);
        return true;
    }

    public virtual bool TryDequeue([MaybeNullWhen(false)] out TItem item) => _queue.TryDequeue(out item, out _);

    public virtual bool TryPeek([MaybeNullWhen(false)] out TItem item) => _queue.TryPeek(out item, out _);
}
