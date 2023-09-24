namespace Lab2.Network.TimeProviders;

public class ConstantTimeProvider : IProcessingTimeProvider
{
    private readonly float _processingTime;

    public ConstantTimeProvider(float processingTime) => _processingTime = processingTime;

    public float GetProcessingTime() => _processingTime;
}
