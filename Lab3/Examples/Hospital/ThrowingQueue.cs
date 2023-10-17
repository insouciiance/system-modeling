using System;
using System.Diagnostics.CodeAnalysis;
using Lab3.Collections;

namespace Lab3.Examples.Hospital;

public class ThrowingQueue<T> : IQueue<T>
{
    public int Count => 0;

    public bool TryEnqueue(T item) => throw new NotSupportedException();

    public bool TryDequeue([MaybeNullWhen(false)] out T item)
    {
        item = default;
        return false;
    }

    public bool TryPeek([MaybeNullWhen(false)] out T item)
    {
        item = default;
        return false;
    }
}
