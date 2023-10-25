using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Lab4.Network.Selectors;

public class WeightedNodeSelector<T> : INetworkNodeSelector<T>
{
    private readonly ImmutableArray<(NetworkNode<T> Node, float Weight)> _nodes;

    public WeightedNodeSelector(params (NetworkNode<T> Node, float Weight)[] nodes) 
        : this(nodes.ToImmutableArray())
    { }

    public WeightedNodeSelector(ImmutableArray<(NetworkNode<T> Node, float Weight)> nodes)
    {
        float weightsTotal = nodes.Sum(n => n.Weight);

        if (Math.Abs(weightsTotal - 1) > 0.0001f)
            throw new ArgumentException("Sum of weights should be 1", nameof(nodes));

        _nodes = nodes;
    }

    public NetworkNode<T> GetNext(ref T _)
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
