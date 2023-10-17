using System;

namespace Lab3.Network.TimeProviders;

public class ExponentialTimeProvider<T> : IProcessingTimeProvider<T>
{
    private readonly float _mean;

    public ExponentialTimeProvider(float mean) => _mean = mean;

    public float GetProcessingTime(T _)
    {
        float rnd = Random.Shared.NextSingle();

        if (rnd == 0)
            rnd = float.Epsilon;
        
        return -_mean * MathF.Log(rnd);
    }
}
