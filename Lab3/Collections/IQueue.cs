namespace Lab3.Collections;

public interface IQueue<T>
{
    int Count { get; }

    bool TryEnqueue(T item);

    bool TryDequeue(out T item);
}
