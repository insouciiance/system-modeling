using System;
using System.Text;
using Lab2.Network;
using Lab2.Network.Selectors;
using Lab2.Network.TimeProviders;

namespace Lab2;

internal class Program
{
    public static void Main(string[] args)
    {
        NetworkModel model = SetupModel();
        model.Simulate(100);
    }

    private static NetworkModel SetupModel()
    {
        ProcessNode process2 = new(new ConstantProcessingTimeProvider(1f), 5)
        {
            DebugName = "Process2"
        };

        ProcessNode process1 = new(new ConstantProcessingTimeProvider(1f), 5, new WeightedNodeSelector((process2, 1)))
        {
            DebugName = "Process1"
        };

        CreateNode create1 = new(new ConstantProcessingTimeProvider(.95f), process1)
        {
            DebugName = "Create1"
        };

        NetworkModel model = new(process1, process2, create1);
        return model;
    }
}
