using System;
using System.Linq;
using ScottPlot;
using ScottPlot.Statistics;

namespace Lab1;

public class NumberGeneratorProcessor
{
    private const int NUMBERS_COUNT = 10000;

    private const int BINS_COUNT = 100;

    private readonly INumberGenerator _generator;

    public NumberGeneratorProcessor(INumberGenerator generator)
    {
        _generator = generator;
    }

    public void Run()
    {
        double[] numbers = GenerateNumbers();
        PrintStats(numbers);
        ShowPlot(numbers);
        RunHypothesisTest(numbers);
    }

    private double[] GenerateNumbers()
    {
        double[] numbers = new double[NUMBERS_COUNT];

        for (int i = 0; i < NUMBERS_COUNT; i++)
            numbers[i] = _generator.Generate();

        return numbers;
    }

    private void PrintStats(double[] numbers)
    {
        double avg = numbers.Average();
        double variance = numbers.Variance();

        Console.WriteLine($"Average: {avg}");
        Console.WriteLine($"Variance: {variance}");
    }

    private void ShowPlot(double[] numbers)
    {
        double min = numbers.Min();
        double max = numbers.Max();

        Plot plot = new();
        Histogram hist = new(min, max, BINS_COUNT);

        hist.AddRange(numbers);

        var bar = plot.AddBar(values: hist.Counts, positions: hist.Bins);
        bar.BarWidth = (max - min) / hist.BinCount;
        
        WpfPlotViewer viewer = new(plot);
        viewer.ShowDialog();
    }

    private void RunHypothesisTest(double[] numbers)
    {
        double min = numbers.Min();
        double max = numbers.Max();

        int binsCount = BINS_COUNT;
        double binSize = (max - min) / binsCount;
        int[] counts = new int[binsCount];

        for (int i = 0; i < NUMBERS_COUNT; i++)
        {
            double distanceFromMin = numbers[i] - min;
            int binIndex = (int)(distanceFromMin / binSize);
            binIndex = Math.Clamp(binIndex, 0, binsCount - 1);
            counts[binIndex]++;
        }

        double chi = 0;

        int intervals = 0;
        int lowIndex = 0;
        int freq = 0;

        for (int i = 0; i < binsCount; i++)
        {
            freq += counts[i];

            if (freq < 5 && i != binsCount - 1)
                continue;

            double lowEdge = min + binSize * lowIndex;
            double lowDistribution = _generator.GetCumulativeDistribution(lowEdge);
            
            double highEdge = min + binSize * (i + 1);
            double highDistribution = _generator.GetCumulativeDistribution(highEdge);

            double expectedFreq = NUMBERS_COUNT * (highDistribution - lowDistribution);

            chi += Math.Pow(freq - expectedFreq, 2) / expectedFreq;

            freq = 0;
            lowIndex = i + 1;
            intervals++;
        }

        Console.WriteLine($"Chi Actual: {chi}");
        Console.WriteLine($"Intervals (after merge): {intervals}");
    }
}
