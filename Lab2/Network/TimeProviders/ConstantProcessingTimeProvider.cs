namespace Lab2.Network.TimeProviders;

public class ConstantProcessingTimeProvider : IProcessingTimeProvider
{
    private readonly float _processingTime;

    public ConstantProcessingTimeProvider(float processingTime)
    {
        _processingTime = processingTime;
    }

    public float GetProcessingTime() => _processingTime;
}
