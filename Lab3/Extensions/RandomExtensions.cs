using System;

namespace Lab3.Extensions;

public static class RandomExtensions
{
    public static float NextGaussian(this Random random)
    {
        float u1 = 1 - random.NextSingle();
        float u2 = 1 - random.NextSingle();
        float z = MathF.Sqrt(-2 * MathF.Log(u1)) * MathF.Cos(2 * MathF.PI * u2);
        return z;
    }
}
