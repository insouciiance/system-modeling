using System.Diagnostics.CodeAnalysis;
using BCL = System.Collections.Generic;

namespace Lab4.Collections;

public class Queue<T> : IQueue<T>
{
    private readonly BCL.Queue<T> _queue = new();

    public int Count => _queue.Count;

    public virtual bool TryEnqueue(T item)
    {
        _queue.Enqueue(item);
        return true;
    }

    public virtual bool TryDequeue([MaybeNullWhen(false)] out T item) => _queue.TryDequeue(out item);

    public virtual bool TryPeek([MaybeNullWhen(false)] out T item) => _queue.TryPeek(out item);
}
