using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Lab4.Examples;
using Lab4.Network;

namespace Lab4;

internal class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}

public class Benchmark
{
    private NetworkModel<int> _sequential = null!;

    private NetworkModel<int> _fork = null!;

    [IterationSetup]
    public void PrepareModels()
    {
        _sequential = ModelHelper.CreateSequential(30);
        _fork = ModelHelper.CreateFork(5, 6);
    }

    [Benchmark]
    [Arguments(1_000)]
    [Arguments(2_000)]
    [Arguments(3_000)]
    [Arguments(4_000)]
    [Arguments(5_000)]
    [Arguments(6_000)]
    [Arguments(7_000)]
    [Arguments(8_000)]
    [Arguments(9_000)]
    [Arguments(10_000)]
    [Arguments(11_000)]
    [Arguments(12_000)]
    [Arguments(13_000)]
    [Arguments(14_000)]
    [Arguments(15_000)]
    public void Sequential(int time)
    {
        _sequential.Simulate(time);
    }

    [Benchmark]
    [Arguments(1_000)]
    [Arguments(2_000)]
    [Arguments(3_000)]
    [Arguments(4_000)]
    [Arguments(5_000)]
    [Arguments(6_000)]
    [Arguments(7_000)]
    [Arguments(8_000)]
    [Arguments(9_000)]
    [Arguments(10_000)]
    [Arguments(11_000)]
    [Arguments(12_000)]
    [Arguments(13_000)]
    [Arguments(14_000)]
    [Arguments(15_000)]
    public void Fork(int time)
    {
        _fork.Simulate(time);
    }
}
