using System;

namespace Lab1;

public class NumberGenerator1 : INumberGenerator
{
    private const double LAMBDA = 10;

    public double Generate()
    {
        double rnd = Random.Shared.NextDouble();
        return -Math.Log(rnd) / LAMBDA;
    }

    public double GetCumulativeDistribution(double x)
    {
        return 1 - Math.Pow(Math.E, -LAMBDA * x);
    }
}
