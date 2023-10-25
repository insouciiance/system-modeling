namespace Lab4.Network.Processors;

public interface INetworkNodeProcessor<T>
{
    T? Current { get; }

    float CompletionTime { get; }

    bool TryEnter(T item);

    bool TryExit();

    void CurrentTimeUpdated(float currentTime);

    void DebugPrint() { }
}
