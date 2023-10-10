namespace Lab3.Network.TimeProviders;

public class UniformTimeProvider : IProcessingTimeProvider
{
    private readonly float _min;
    private readonly float _max;

    public UniformTimeProvider(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public float GetProcessingTime() => _min + Random.Shared.NextSingle() * (_max - _min);
}
