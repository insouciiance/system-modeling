namespace Lab3.Collections;

public sealed class LimitedQueue<T> : Queue<T>
{
    private readonly int _maxSize;

    public LimitedQueue(int maxSize) => _maxSize = maxSize;

    public override bool TryEnqueue(T item) => Count < _maxSize && base.TryEnqueue(item);
}
