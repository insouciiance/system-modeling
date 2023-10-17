using System;
using Lab3.Collections;
using Lab3.Network;
using Lab3.Network.Factories;
using Lab3.Network.Processors;
using Lab3.Network.Selectors;
using Lab3.Network.TimeProviders;

namespace Lab3.Examples.Bank;

public static class BankHelper
{
    public static NetworkModel<int> CreateModel()
    {
        WindowTimeProvider windowTimeProvider = new(
            new NormalTimeProvider<int>(1, 0.3f),
            new ExponentialTimeProvider<int>(0.3f));

        AccumulatorTimeProvider<int> windowTimeDecorator = new(windowTimeProvider);

        LimitedWorkStealingQueue<int> q1 = new(3, 2);
        LimitedWorkStealingQueue<int> q2 = new(3, 2);

        q1.Link(q2);
        q2.Link(q1);

        q1.TryEnqueue(0);
        q2.TryEnqueue(0);
        
        q1.TryEnqueue(0);
        q2.TryEnqueue(0);

        DisposeNode<int> exit = new();

        SingleNodeProcessor<int> processor1 = new(windowTimeDecorator);
        processor1.TryEnter(0);
        ProcessNode<int> window1 = new(processor1, q1)
        {
            DebugName = "Window 1",
            NextNodeSelector = new ConstantNodeSelector<int>(exit)
        };

        SingleNodeProcessor<int> processor2 = new(windowTimeDecorator);
        processor2.TryEnter(0);
        ProcessNode<int> window2 = new(processor2, q2)
        {
            DebugName = "Window 2",
            NextNodeSelector = new ConstantNodeSelector<int>(exit)
        };

        BankWindowSelector<int> windowSelector = new(window1, window2);

        ExponentialTimeProvider<int> carTimeProvider = new(0.5f);

        IncrementalJobFactory<int> factory = new();

        CreateNode<int> cars = new(factory, carTimeProvider, windowSelector, 0.1f)
        {
            DebugName = "Cars"
        };

        NetworkModel<int> model = new(cars, window1, window2, exit);

        model.DebugPrintExtra += () =>
        {
            float avgClientCount = window1.QueueAverageSize + window2.QueueAverageSize + processor1.AverageLoad + processor2.AverageLoad;
            Console.WriteLine($"Average client count: {avgClientCount}");
        };

        model.DebugPrintExtra += () =>
        {
            float failedClients = window1.FailuresCount + window2.FailuresCount;
            float avgFailRate = failedClients / cars.ProcessedCount;
            Console.WriteLine($"% of failed clients: {avgFailRate * 100}");
        };

        model.DebugPrintExtra += () =>
        {
            float totalTimeInBank = window1.QueueWaitingTimeTotal + window2.QueueWaitingTimeTotal + windowTimeDecorator.TotalProcessingTime;
            float avgTimeInBank = totalTimeInBank / cars.ProcessedCount;
            Console.WriteLine($"Average time spent in bank: {avgTimeInBank}");
        };

        return model;
    }
}
