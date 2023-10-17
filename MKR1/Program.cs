using Lab3.Collections;
using Lab3.Network;
using Lab3.Network.Factories;
using Lab3.Network.Processors;
using Lab3.Network.Selectors;
using Lab3.Network.TimeProviders;

namespace MKR1;

internal class Program
{
    static void Main(string[] args)
    {
        var model = CreateBank();
        model.Simulate(1000);
    }

    private static NetworkModel<Client> CreateBank()
    {
        ClientStatRecorder recorder = new();

        DisposeNode<Client> exit = new();
        exit.OnEnter += recorder.RecordExit;

        Lab3.Collections.Queue<Client> cashierQueue = new();
        UniformTimeProvider<Client> cashierTimeProvider = new(2, 4);
        ConstantNodeSelector<Client> cashierSelector = new(exit);
        SingleNodeProcessor<Client> cashierChannel = new(cashierTimeProvider);
        ProcessNode<Client> cashier = new(cashierChannel, cashierQueue)
        {
            NextNodeSelector = cashierSelector
        };

        LimitedQueue<Client> operatorsQueue = new(10);
        UniformTimeProvider<Client> operatorsTimeProvider = new(2, 10);
        ConstantNodeSelector<Client> operatorsSelector = new(cashier);
        CompositeNodeProcessor<Client> operatorsChannels = new(
            new SingleNodeProcessor<Client>(operatorsTimeProvider),
            new SingleNodeProcessor<Client>(operatorsTimeProvider));

        ProcessNode<Client> operators = new(operatorsChannels, operatorsQueue)
        {
            NextNodeSelector = operatorsSelector
        };

        operators.OnEnter += recorder.RecordEnter;

        ConstantNodeSelector<Client> clientsSelector = new(operators);
        ExponentialTimeProvider<Client> clientsTimeProvider = new(3);
        CreateNode<Client> clients = new(
            new ClientFactory(),
            clientsTimeProvider,
            clientsSelector);

        NetworkModel<Client> model = new(clients, operators, cashier, exit);

        model.DebugPrintExtra += () =>
        {
            Console.WriteLine($"Average client time in bank: {recorder.AverageClientTime}");
        };

        return model;
    }
}
