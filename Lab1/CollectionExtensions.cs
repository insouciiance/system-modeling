using System;
using System.Linq;

namespace Lab1;

public static class CollectionExtensions
{
    public static double Variance(this double[] values)
    {
        double avg = values.Average();
        return values.Select(x => Math.Pow(x - avg, 2)).Sum() / values.Length;
    }
}
