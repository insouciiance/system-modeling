using System.Collections.Immutable;
using System.Diagnostics;

namespace Lab3.Network.Processors;

public class CompositeNodeProcessor : INetworkNodeProcessor
{
    public float CompletionTime => _nodes.Min(n => n.CompletionTime);

    private readonly ImmutableArray<INetworkNodeProcessor> _nodes;

    public CompositeNodeProcessor(params INetworkNodeProcessor[] nodes)
        : this(nodes.ToImmutableArray()) { }

    public CompositeNodeProcessor(ImmutableArray<INetworkNodeProcessor> nodes)
    {
        _nodes = nodes;
    }

    public bool TryEnter()
    {
        foreach (var node in _nodes)
        {
            if (node.TryEnter())
                return true;
        }

        return false;
    }

    public bool TryExit()
    {
        foreach (var node in _nodes)
        {
            if (Math.Abs(node.CompletionTime - CompletionTime) < 0.0001f)
                return node.TryExit();
        }

        throw new UnreachableException();
    }

    public void CurrentTimeUpdated(float currentTime)
    {
        foreach (var node in _nodes)
            node.CurrentTimeUpdated(currentTime);
    }
}
