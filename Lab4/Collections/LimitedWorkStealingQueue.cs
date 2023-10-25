namespace Lab4.Collections;

public sealed class LimitedWorkStealingQueue<T> : WorkStealingQueue<T>
{
    private readonly int _maxSize;

    public LimitedWorkStealingQueue(int maxSize, int sizeDelta)
        : base(sizeDelta) => _maxSize = maxSize;

    public override bool TryEnqueue(T item) => Count < _maxSize && base.TryEnqueue(item);
}
