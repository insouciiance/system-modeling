using System;
using Lab2.Extensions;

namespace Lab2.Network.TimeProviders;

public class NormalTimeProvider : IProcessingTimeProvider
{
    private readonly float _mean;
    private readonly float _stdDev;

    public NormalTimeProvider(float mean, float stdDev)
    {
        _mean = mean;
        _stdDev = stdDev;
    }

    public float GetProcessingTime() => Random.Shared.NextGaussian() * _stdDev + _mean;
}
