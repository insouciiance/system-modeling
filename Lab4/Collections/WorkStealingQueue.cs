using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Lab4.Collections;

public class WorkStealingQueue<T> : IQueue<T>
{
    private event Action? OnChanged;

    private readonly int _sizeDelta;

    private readonly List<T> _list = new();

    private int _stolenCount;

    public int Count => _list.Count;

    public WorkStealingQueue(int sizeDelta)
    {
        _sizeDelta = sizeDelta;
    }

    public virtual bool TryEnqueue(T item)
    {
        _list.Add(item);
        OnChanged?.Invoke();
        return true;
    }

    public virtual bool TryDequeue([MaybeNullWhen(false)] out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        item = _list[^1];
        _list.RemoveAt(_list.Count - 1);
        OnChanged?.Invoke();
        return true;
    }

    public virtual bool TryPeek([MaybeNullWhen(false)] out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        item = _list[^1];
        return true;
    }

    public virtual void DebugPrint()
    {
        Debug.WriteLine($"{nameof(WorkStealingQueue<int>)} stole {_stolenCount} items");
    }

    public void Link(WorkStealingQueue<T> other)
    {
        other.OnChanged += () => HandleChange(other);
        OnChanged += () => HandleChange(other);
    }

    private void HandleChange(WorkStealingQueue<T> other)
    {
        if (other.Count - Count < _sizeDelta)
            return;

        Debug.WriteLine($"Stealing work from Q size {other.Count} to Q size {Count}");

        T item = other._list[0];
            
        if (TryEnqueue(item))
            other._list.RemoveAt(0);

        _stolenCount++;
    }
}
