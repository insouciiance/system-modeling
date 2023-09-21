using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Lab2.Network.Selectors;

public class WeightedNodeSelector : INetworkNodeSelector
{
    private readonly ImmutableArray<(NetworkNode Node, float Weight)> _nodes;

    public WeightedNodeSelector(params (NetworkNode Node, float Weight)[] nodes) 
        : this(nodes.ToImmutableArray())
    { }

    public WeightedNodeSelector(ImmutableArray<(NetworkNode Node, float Weight)> nodes)
    {
        float weightsTotal = nodes.Sum(n => n.Weight);

        if (Math.Abs(weightsTotal - 1) > 0.0001f)
            throw new ArgumentException("Sum of weights should be 1", nameof(nodes));

        _nodes = nodes;
    }

    public NetworkNode GetNext()
    {
        float acc = 0;
        float rndWeight = Random.Shared.NextSingle();

        foreach (var (node, weight) in _nodes)
        {
            acc += weight;

            if (rndWeight < acc)
                return node;
        }

        throw new UnreachableException();
    }
}
