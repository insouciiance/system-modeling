using Lab2.Network;
using Lab2.Network.Processors;
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
        ProcessNode process3 = new(new SingleNodeProcessor(new ConstantTimeProvider(1)), 5)
        {
            DebugName = "Process3"
        };

        ProcessNode process2 = new(new SingleNodeProcessor(new ConstantTimeProvider(1)), 5)
        {
            DebugName = "Process2",
        };

        ProcessNode process1 = new(new SingleNodeProcessor(new ConstantTimeProvider(1)), 5)
        {
            DebugName = "Process1"
        };

        CreateNode create1 = new(new ConstantTimeProvider(1), process1)
        {
            DebugName = "Create1"
        };

        process1.NextNodeSelector = new WeightedNodeSelector((process2, 1));
        process2.NextNodeSelector = new WeightedNodeSelector((process3, 1));

        NetworkModel model = new(create1, process1, process2, process3);
        return model;
    }
}
