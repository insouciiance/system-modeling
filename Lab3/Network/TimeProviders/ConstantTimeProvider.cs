namespace Lab3.Network.TimeProviders;

public class ConstantTimeProvider<T> : IProcessingTimeProvider<T>
{
    private readonly float _processingTime;

    public ConstantTimeProvider(float processingTime) => _processingTime = processingTime;

    public float GetProcessingTime(T _) => _processingTime;
}
