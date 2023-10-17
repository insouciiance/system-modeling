namespace Lab3.Network.TimeProviders;

public class AccumulatorTimeProvider<T> : IProcessingTimeProvider<T>
{
    private readonly IProcessingTimeProvider<T> _wrappee;

    public float TotalProcessingTime { get; private set; }

    public AccumulatorTimeProvider(IProcessingTimeProvider<T> wrappee) => _wrappee = wrappee;

    public float GetProcessingTime(T item)
    {
        float time = _wrappee.GetProcessingTime(item);
        TotalProcessingTime += time;
        return time;
    }
}
