using System;

namespace Lab1;

public class NumberGenerator2 : INumberGenerator
{
    private const double A = 0;

    private const double SIGMA = 1.0;

    public double Generate()
    {
        double u = 0;

        for (int i = 0; i < 12; i++)
            u += Random.Shared.NextDouble();

        u -= 6;

        return SIGMA * u + A;
    }

    public double GetCumulativeDistribution(double x)
    {
        double erf = MathHelper.Erf((x - A) / (SIGMA * Math.Sqrt(2)));
        return (1 + erf) / 2;
    }
}
