using System.Numerics;

namespace Lab3.Network.Factories;

public class IncrementalJobFactory<T> : IJobFactory<T>
    where T : INumber<T>
{
    private T _current = T.Zero;

    public T Create() => _current += T.One;
}
