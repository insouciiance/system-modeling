using System;
using Lab3.Extensions;

namespace Lab3.Network.TimeProviders;

public class NormalTimeProvider<T> : IProcessingTimeProvider<T>
{
    private readonly float _mean;
    private readonly float _stdDev;

    public NormalTimeProvider(float mean, float stdDev)
    {
        _mean = mean;
        _stdDev = stdDev;
    }

    public float GetProcessingTime(T _) => Random.Shared.NextGaussian() * _stdDev + _mean;
}
