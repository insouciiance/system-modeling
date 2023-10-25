namespace Lab4.Network.TimeProviders;

public interface IProcessingTimeProvider<T>
{
    float GetProcessingTime(T item);
}
