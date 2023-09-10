namespace Lab1;

public class NumberGenerator3 : INumberGenerator
{
    private const double A = 1220703125;

    private const double C = 1L << 31;

    private double _prev = 1;

    public double Generate()
    {
        _prev = A * _prev % C;
        return _prev / C;
    }

    public double GetCumulativeDistribution(double x)
    {
        return x switch
        {
            < 0 => 0,
            >=0 and <= 1 => x,
            > 1 => 1
        };
    }
}
