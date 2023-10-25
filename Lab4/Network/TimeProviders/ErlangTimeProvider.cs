using System;
using MathNet.Numerics.Distributions;

namespace Lab4.Network.TimeProviders;

public class ErlangTimeProvider<T> : IProcessingTimeProvider<T>
{
    private readonly Erlang _erlang;

    public ErlangTimeProvider(int k, float mean)
    {
        _erlang = new(k, mean, Random.Shared);
    }

    public float GetProcessingTime(T _) => (float)_erlang.Sample();
}
