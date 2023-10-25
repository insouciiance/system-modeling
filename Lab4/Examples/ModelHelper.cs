using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lab4.Network;
using Lab4.Network.Factories;
using Lab4.Network.Processors;
using Lab4.Network.Selectors;
using Lab4.Network.TimeProviders;

namespace Lab4.Examples;

public static class ModelHelper
{
    public static NetworkModel<int> CreateSequential(int processCount)
    {
        List<NetworkNode<int>> nodes = new();

        ExponentialTimeProvider<int> timeProvider = new(1);

        for (int i = 0; i < processCount; i++)
        {
            ProcessNode<int> processNode = new(new SingleNodeProcessor<int>(timeProvider), new Collections.Queue<int>())
            {
                NextNodeSelector = nodes.Count > 0 ? new ConstantNodeSelector<int>(nodes[^1]) : null
            };

            processNode.Enter(0);

            nodes.Add(processNode);
        }

        CreateNode<int> createNode = new(new IncrementalJobFactory<int>(), timeProvider, new ConstantNodeSelector<int>(nodes[^1]));
        nodes.Add(createNode);

        NetworkModel<int> model = new(nodes.ToImmutableArray());
        return model;
    }

    public static NetworkModel<int> CreateFork(int forkCount, int processCountInFork)
    {
        List<NetworkNode<int>> nodes = new();

        ExponentialTimeProvider<int> timeProvider = new(1);

        var forks = new ProcessNode<int>[forkCount];

        for (int i = 0; i < forkCount; i++)
        {
            for (int j = 0; j < processCountInFork; j++)
            {
                ProcessNode<int> processNode = new(new SingleNodeProcessor<int>(timeProvider), new Collections.Queue<int>())
                {
                    NextNodeSelector = forks[i] is not null ? new ConstantNodeSelector<int>(forks[i]) : null
                };

                processNode.Enter(0);

                nodes.Add(processNode);
                forks[i] = processNode;
            }
        }

        CreateNode<int> createNode = new(
            new IncrementalJobFactory<int>(),
            timeProvider,
            new WeightedNodeSelector<int>(forks.Select(n => ((NetworkNode<int>)n, 1f / forkCount)).ToImmutableArray()));
        nodes.Add(createNode);

        NetworkModel<int> model = new(nodes.ToImmutableArray());
        return model;
    }
}
