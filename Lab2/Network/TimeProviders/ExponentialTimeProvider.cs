using System;

namespace Lab2.Network.TimeProviders;

public class ExponentialTimeProvider : IProcessingTimeProvider
{
    private readonly float _mean;

    public ExponentialTimeProvider(float mean) => _mean = mean;

    public float GetProcessingTime()
    {
        float rnd = Random.Shared.NextSingle();

        if (rnd == 0)
            rnd = float.Epsilon;

        return -_mean * MathF.Log(rnd);
    }
}
